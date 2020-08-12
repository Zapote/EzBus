namespace EzBus.RabbitMQ
{
    internal class ConsumerFactory : IConsumerFactory
    {
        private readonly IChannelFactory channelFactory;
        private readonly IAddressConfig conf;

        public ConsumerFactory(IChannelFactory channelFactory, IAddressConfig conf)
        {
            this.channelFactory = channelFactory;
            this.conf = conf;
        }

        public IConsumer Create()
        {
            return new Consumer(channelFactory, conf.Address);
        }
    }
}
