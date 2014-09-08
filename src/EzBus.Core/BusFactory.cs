using EzBus.Core.Routing;

namespace EzBus.Core
{
    public class BusFactory : IBusFactory, IBusStarter
    {
        private readonly HostConfig config = new HostConfig();

        public IHostConfig Config { get { return config; } }

        public IBus Start()
        {
            var host = new Host(config);
            host.Start();
            return CreateBus();
        }

        private Bus CreateBus()
        {
            return new Bus(config.SendingChannel, new ConfigurableMessageRouting(), new InMemorySubscriptionStorage());
        }

        public static IBusFactory Setup()
        {
            return new BusFactory();
        }
    }
}