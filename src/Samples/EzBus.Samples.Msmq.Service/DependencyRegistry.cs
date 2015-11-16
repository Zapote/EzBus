using EzBus.ObjectFactory;
using EzBus.Samples.Msmq.Service.Fwk;

namespace EzBus.Samples.Msmq.Service
{
    public class DependencyRegistry : ServiceRegistry
    {
        public DependencyRegistry()
        {
            Register<IOrderNumberGenerator, OrderNumberGenerator>().As.Singleton();
        }
    }
}