using EzBus.ObjectFactory;
using EzBus.WindowsAzure.ServiceBus.Subscription;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class ServiceBusRegistry : ServiceRegistry
    {
        public ServiceBusRegistry()
        {
            Register<ISubscriptionStorage, AzureTableSubscriptionStorage>().As.Singleton();
        }
    }
}
