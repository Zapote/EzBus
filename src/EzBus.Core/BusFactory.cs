using EzBus.Core.Logging;
using EzBus.Core.Routing;
using EzBus.Logging;

namespace EzBus.Core
{
    public class BusFactory : IBusStarter
    {
        private readonly HostConfig config = new HostConfig();
        private Host host;
        public HostConfig Config { get { return config; } }

        public IBus Start()
        {
            HostLogManager.Configure(new TraceHostLoggerFactory(), LogLevel.Debug);

            host = new Host(config);
            host.Start();
            return CreateBus();
        }

        private static CoreBus CreateBus()
        {
            var sendingChannel = MessageChannelResolver.GetSendingChannel();
            return new CoreBus(sendingChannel, new ConfigurableMessageRouting(), new InMemorySubscriptionStorage());
        }

        public static BusFactory Setup()
        {
            return new BusFactory();
        }
    }
}