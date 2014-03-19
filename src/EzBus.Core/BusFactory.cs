using EzBus.Core.Routing;

namespace EzBus.Core
{
    public class BusFactory : IBusFactory, IBusStarter
    {
        private readonly EndpointConfig config = new EndpointConfig();

        public IEndpointConfig Config { get { return config; } }

        public IBus SendOnly()
        {
            return CreateBus();
        }

        public IBus Start()
        {
            var host = new EndpointHost(config.ReceivingChannel);
            host.Start();
            return CreateBus();
        }

        private Bus CreateBus()
        {
            return new Bus(config.SendingChannel, new ConfigurableMessageRouting());
        }

        public static IBusFactory Setup()
        {
            return new BusFactory();
        }
    }
}