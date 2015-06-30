using EzBus.Core.Builders;
using EzBus.Samples.Msmq.Service.Fwk;

namespace EzBus.Samples.Msmq.Service
{
    public class DependencyRegistry : ServiceRegistry
    {
        public DependencyRegistry()
        {
            Register<IClientGreeter, ClientGreeter>().As.Singleton();
        }
    }
}