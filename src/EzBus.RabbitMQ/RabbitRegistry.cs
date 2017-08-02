using EzBus.ObjectFactory;

namespace EzBus.RabbitMQ
{
    public class RabbitRegistry : ServiceRegistry
    {
        public RabbitRegistry()
        {
            Register<IChannelFactory, ChannelFactory>().As.Singleton();
            Register<IRabbitMQConfig, RabbitMQConfig>().As.Singleton();
            Register<ITransport, RabbitMQTransport>().As.Singleton();
        }
    }
}
