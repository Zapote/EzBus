using System.Collections.Generic;

namespace EzBus.Core.Builders
{
    public abstract class ServiceRegistry : IServiceRegistry
    {
        private readonly IList<RegistryInstance> instances = new List<RegistryInstance>();

        protected ServiceRegistry Register<TService, TImplementation>() where TImplementation : TService
        {
            instances.Add(new RegistryInstance(typeof(TService), typeof(TImplementation)));
            return this;
        }

        public IEnumerable<RegistryInstance> Instances
        {
            get { return instances; }
        }
    }
}