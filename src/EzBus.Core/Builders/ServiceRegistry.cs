using System.Collections.Generic;

namespace EzBus.Core.Builders
{
    public abstract class ServiceRegistry : IServiceRegistry
    {
        private readonly IList<RegistryInstance> instances = new List<RegistryInstance>();

        protected IConfigureLifeCycle Register<TService, TImplementation>() where TImplementation : TService
        {
            var registryInstance = new RegistryInstance(typeof(TService), typeof(TImplementation));
            instances.Add(registryInstance);
            return registryInstance;
        }

        public IEnumerable<RegistryInstance> Instances
        {
            get { return instances; }
        }
    }
}