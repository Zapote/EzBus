using System;
using System.Collections.Generic;

namespace EzBus.ObjectFactory
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

        protected void RegisterInstance(Type serviceType, object instance)
        {
            var registryInstance = new RegistryInstance(serviceType, instance);
            registryInstance.Singleton();
            instances.Add(registryInstance);
        }

        public IEnumerable<IRegistryInstance> Instances => instances;
    }
}