using EzBus;
using EzBus.Core;
using EzBus.Core.ObjectFactory;
using EzBus.Core.Resolvers;
using EzBus.Logging;
using EzBus.ObjectFactory;

// ReSharper disable once CheckNamespace
public static class Bus
{
    private static readonly IBus bus;
    private static Host host;
    private static readonly HostCustomization hostCustomization = new HostCustomization();
    private static readonly DefaultObjectFactory objectFactory;

    static Bus()
    {
        if (bus != null) return;

        ConfigureLogging();
        objectFactory = new DefaultObjectFactory();
        objectFactory.Initialize();

        bus = objectFactory.GetInstance<IBus>();
    }

    public static void Start()
    {
        host = new HostFactory().Build(objectFactory.GetInstance<ITaskRunner>());
        host.Start();
    }

    public static HostCustomization Configure()
    {
        return hostCustomization;
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

    public class HostCustomization
    {
        public IHostConfig HostConfig { get; }
        public IObjectFactory ObjectFactory { get; }

        public HostCustomization()
        {
            HostConfig = new HostConfig();
        }

        public HostCustomization WorkerThreads(int workerThreads)
        {
            HostConfig.WorkerThreads = workerThreads;
            return this;
        }

        public HostCustomization NumberOfRetrys(int numberOfRetrys)
        {
            HostConfig.NumberOfRetrys = numberOfRetrys;
            return this;
        }

        public HostCustomization EndpointName(string endpointName)
        {
            HostConfig.EndpointName = endpointName;
            return this;
        }

        public HostCustomization ErrorEndpointName(string errorEndpointName)
        {
            HostConfig.ErrorEndpointName = errorEndpointName;
            return this;
        }
    }
}

