using System;
using EzBus;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Core.Resolvers;
using EzBus.Logging;
using EzBus.ObjectFactory;

// ReSharper disable once CheckNamespace
public static class Bus
{
    private static readonly IObjectFactory objectFactory = new DefaultObjectFactory();
    private static readonly IBusConfig busConfig = new BusConfig();
    private static IBus bus;

    public static void Start(Action<IBusConfig> configAction = null)
    {
        objectFactory.Initialize();

        ConfigureLogging();
        SetupBusConfig(configAction);
        CreateBus();
        StartHost();
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

    private static void ConfigureLogging()
    {
        var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();
        LogManager.Configure(loggerFactory, LogLevel.Debug);
    }

    private static void SetupBusConfig(Action<IBusConfig> configAction)
    {
        configAction?.Invoke(busConfig);
        objectFactory.RegisterInstance(typeof(IBusConfig), busConfig);
    }

    private static void CreateBus()
    {
        bus = objectFactory.GetInstance<IBus>();
    }

    private static void StartHost()
    {
        var host = objectFactory.GetInstance<Host>();
        host.Start();
    }
}

