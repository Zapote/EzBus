using EzBus.Core.Routing;

namespace EzBus.Core
{
    public class BusFactory : IBusStarter
    {
        private readonly HostConfig config = new HostConfig();
        private Host host;

        public HostConfig Config { get { return config; } }

        public IBus Start()
        {
            host = new Host(config);
            host.Start();
            return CreateBus();
        }

        private static Bus CreateBus()
        {
            var sendingChannel = MessageChannelResolver.GetSendingChannel();
            return new Bus(sendingChannel, new ConfigurableMessageRouting(), new InMemorySubscriptionStorage());
        }

        public static BusFactory Setup()
        {
            return new BusFactory();
        }
    }
}