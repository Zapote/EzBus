using System;
using System.Runtime.InteropServices.ComTypes;
using EzBus;
using EzBus.Core.Resolvers;
using EzBus.ObjectFactory;
using EzBus.Utils;

// ReSharper disable once CheckNamespace
public sealed class Bus
{
    private IObjectFactory objectFactory;
    private IBus bus;
    private static volatile Bus instance;
    private static readonly object syncRoot = new object();

    private Bus() { }

    private static Bus Instance
    {
        get
        {
            if (instance != null) return instance;

            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new Bus();
                }
            }

            return instance;
        }
    }

    public static ITransport Configure(Action<IBusConfig> action = null)
    {
        Instance.InitializeObjectFactory();

        var config = Instance.GetConfig();
        action?.Invoke(config);

        var transport = Instance.GetTransport();

        return transport;
    }

    public static void Stop()
    {
        var transport = Instance.GetTransport();
        transport.Host.Stop();
    }

    public static void Send(string destination, object message)
    {
        Instance.GetBus().Send(destination, message);
    }

    public static void Publish(object message)
    {
        Instance.GetBus().Publish(message);
    }

    /// <summary>
    /// Subscribe to published messages from an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to subscribe to</param>
    /// <param name="messageName">Name of the message. Default empty string (all messages)</param>
    public static void Subscribe(string endpoint, string messageName = "")
    {
        var sm = Instance.objectFactory.GetInstance<ISubscriptionManager>();
        sm?.Subscribe(endpoint, messageName);
    }

    /// <summary>
    /// Subscribe to published messages from an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to subscribe to</param>
    public static void Subscribe<T>(string endpoint)
        where T : class
    {
        var messageName = typeof(T).Name;
        var sm = Instance.objectFactory.GetInstance<ISubscriptionManager>();
        sm?.Subscribe(endpoint, messageName);
    }

    /// <summary>
    /// Unsubscribe from published messages from an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to subscribe to</param>
    public static void Unsubscribe<T>(string endpoint)
        where T : class
    {
        var messageName = typeof(T).Name;
        var sm = Instance.objectFactory.GetInstance<ISubscriptionManager>();
        sm?.Unsubscribe(endpoint, messageName);
    }

    /// <summary>
    /// Unsubscribe from published messages from an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to subscribe to</param>
    /// <param name="messageName">Name of the message. Default empty string (all messages)</param>
    public static void Unsubscribe(string endpoint, string messageName = "")
    {
        var sm = Instance.objectFactory.GetInstance<ISubscriptionManager>();
        sm?.Unsubscribe(endpoint, messageName);
    }

    private void InitializeObjectFactory()
    {
        var objectFactoryType = TypeResolver.GetType<IObjectFactory>();
        objectFactory = (IObjectFactory)objectFactoryType.CreateInstance();
        objectFactory.Initialize();
    }

    private IBusConfig GetConfig()
    {
        var config = objectFactory.GetInstance<IBusConfig>();
        return config;
    }

    private ITransport GetTransport()
    {
        try
        {
            var transport = objectFactory.GetInstance<ITransport>();
            return transport;
        }
        catch (Exception)
        {
            var host = objectFactory.GetInstance<IHost>();
            return new NullTransport(host);
        }
    }

    private IBus GetBus()
    {
        if (bus != null) return bus;
        if (objectFactory == null) throw new Exception("Transport not configured! Pls first call Bus.Configure.UseRabbitMQ() or Bus.Configure.Msmq()");

        var t = TypeResolver.GetType<IBus>();

        if (t.IsLocal())
        {
            bus = objectFactory.GetInstance<IBus>();
            return bus;
        }

        bus = t.CreateInstance() as IBus;
        return bus;
    }
}

