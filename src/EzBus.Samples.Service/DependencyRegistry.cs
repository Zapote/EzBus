using EzBus.Core.Builders;

namespace EzBus.Samples.Service
{
    public class DependencyRegistry : ServiceRegistry
    {
        public DependencyRegistry()
        {
            Register<IDependency, Dependency>().As.Singelton();
        }
    }
}