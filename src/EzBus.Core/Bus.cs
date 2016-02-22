using System.Threading.Tasks;
using EzBus;
using EzBus.Core;
using EzBus.Core.Resolvers;
using EzBus.Logging;

// ReSharper disable once CheckNamespace
public static class Bus
{
    private static readonly IBus bus;
    private static Host host;
    private static readonly HostCustomization hostCustomization = new HostCustomization();

    static Bus()
    {
        if (bus != null) return;

        ConfigureLogging();

        var factory = new BusFactory();
        bus = factory.Build();
    }

    public static void Start()
    {
        host = new HostFactory().Build(hostCustomization.HostConfig);
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
        public IHostConfig HostConfig { get; set; }

        public HostCustomization()
        {
            HostConfig = ObjectFactoryResolver.Get().GetInstance<IHostConfig>();
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

