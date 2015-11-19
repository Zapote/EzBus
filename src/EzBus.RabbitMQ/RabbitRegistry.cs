using EzBus.ObjectFactory;

namespace EzBus.RabbitMQ
{
    public class RabbitRegistry : ServiceRegistry
    {
        public RabbitRegistry()
        {
            Register<IChannelFactory, ChannelFactory>().As.Singleton();
        }
    }
}
