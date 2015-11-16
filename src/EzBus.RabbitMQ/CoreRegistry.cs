using EzBus.ObjectFactory;

namespace EzBus.RabbitMQ
{
    public class CoreRegistry : ServiceRegistry
    {
        public CoreRegistry()
        {
            Register<IChannelFactory, ChannelFactory>().As.Singleton();
        }
    }
}
