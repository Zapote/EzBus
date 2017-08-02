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

    public static ITransport Configure(Action<IBusConfig> action)
    {
        InitializeObjectFactory();
        
        var transport = GetTransport();
        var config = GetConfig();
        action?.Invoke(config);

        GetBus();

        return transport;
    }

    public static void Send(object message)
    {
        bus.Send(message);
    }

    public static void Send(string destination, object message)
    {
        bus.Send(destination, message);
    }

    public static void Publish(object message)
    {
        bus.Publish(message);
    }

    private static void InitializeObjectFactory()
    {
        var objectFactoryType = TypeResolver.GetType<IObjectFactory>();
        objectFactory = (IObjectFactory)objectFactoryType.CreateInstance();
        objectFactory.Initialize();
    }

    private static void GetBus()
    {
        bus = objectFactory.GetInstance<IBus>();
    }

    private static IBusConfig GetConfig()
    {
        var config = objectFactory.GetInstance<IBusConfig>();
        return config;
    }

    private static ITransport GetTransport()
    {
        var transport = objectFactory.GetInstance<ITransport>();
        return transport;
    }
}

