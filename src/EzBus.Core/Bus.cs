using System;
using EzBus;
using EzBus.Core.Resolvers;
using EzBus.ObjectFactory;
using EzBus.Utils;

// ReSharper disable once CheckNamespace
public static class Bus
{
    private static IObjectFactory objectFactory;
    private static IBus bus;

    public static ITransport Configure(Action<IBusConfig> action = null)
    {
        InitializeObjectFactory();

        var transport = GetTransport();
        var config = GetConfig();
        action?.Invoke(config);

        GetBus();

        return transport;
    }

    public static void Send(string destination, object message)
    {
        VerifyStarted();
        bus.Send(destination, message);
    }

    public static void Publish(object message)
    {
        VerifyStarted();
        bus.Publish(message);
    }

    public static void Subscribe(string endpoint)
    {
        VerifyStarted("Failed to subscribe");
        var sm = objectFactory.GetInstance<ISubscriptionManager>();
        sm?.Subscribe(endpoint);
    }

    private static void InitializeObjectFactory()
    {
        var objectFactoryType = TypeResolver.GetType<IObjectFactory>();
        objectFactory = (IObjectFactory)objectFactoryType.CreateInstance();
        objectFactory.Initialize();
    }

    private static void GetBus()
    {
        var busType = TypeResolver.GetType<IBus>();
        if (busType.IsLocal())
        {
            bus = objectFactory.GetInstance<IBus>();
            return;
        }
        bus = busType.CreateInstance() as IBus;
    }

    private static IBusConfig GetConfig()
    {
        var config = objectFactory.GetInstance<IBusConfig>();
        return config;
    }

    private static ITransport GetTransport()
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

    private static void VerifyStarted(string failMessage = "")
    {
        if (bus != null) return;

        var message = "EzBus not started!";
        if (failMessage.HasValue())
        {
            message = $"{failMessage} - {message}";
        }
        throw new InvalidOperationException(message);
    }
}

