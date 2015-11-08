using System;
using System.Collections.Generic;

namespace EzBus.Core.Builders
{
    public abstract class ServiceRegistry : IServiceRegistry
    {
        private readonly IList<RegistryInstance> instances = new List<RegistryInstance>();

        protected ILifeCycleConfiguration Register<TService, TImplementation>() where TImplementation : TService
        {
            return Register(typeof(TService), typeof(TImplementation));
        }

        protected ILifeCycleConfiguration Register(Type serviceType, Type implementationType)
        {
            var registryInstance = new RegistryInstance(serviceType, implementationType);
            instances.Add(registryInstance);
            return registryInstance;
        }

        public IEnumerable<RegistryInstance> Instances => instances;
    }
}