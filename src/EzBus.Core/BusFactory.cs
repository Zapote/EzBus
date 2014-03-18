namespace EzBus.Core
{
    public class BusFactory : IBusFactory, IBusStarter
    {
        private readonly EndpointConfig config = new EndpointConfig();

        public IEndpointConfig Config { get { return config; } }

        public IBus SendOnly()
        {
            return new Bus(config.SendingChannel);
        }

        public IBus Start()
        {
            var host = new EndpointHost(config);
            host.Start();
            return new Bus(config.SendingChannel);
        }

        public static IBusFactory Setup()
        {
            return new BusFactory();
        }
    }
}