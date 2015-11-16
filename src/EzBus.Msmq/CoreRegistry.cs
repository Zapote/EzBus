using EzBus.Msmq.Subscription;
using EzBus.ObjectFactory;

namespace EzBus.Msmq
{
    public class CoreRegistry : ServiceRegistry
    {
        public CoreRegistry()
        {
            Register<IMsmqSubscriptionStorage, MsmqSubscriptionStorage>().As.Singleton();
        }
    }
}
