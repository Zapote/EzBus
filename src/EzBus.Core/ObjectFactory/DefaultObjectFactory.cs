using System;
using System.Collections.Generic;
using EzBus.Core.ObjectFactory.LightInject;
using EzBus.Core.Utils;
using EzBus.ObjectFactory;

namespace EzBus.Core.ObjectFactory
{
    public class DefaultObjectFactory : IObjectFactory
    {
        private readonly IServiceContainer container = new ServiceContainer();
        private Scope scope;

        public Guid Id { get; set; } = Guid.NewGuid();

        public object GetInstance(Type type)
        {
            return container.GetInstance(type);
        }

        public T GetInstance<T>() where T : class
        {
            return container.GetInstance<T>();
        }

        public IEnumerable<T> GetInstances<T>() where T : class
        {
            return container.GetAllInstances<T>();
        }

        public void Initialize()
        {
            var registryTypes = new Utils.AssemblyScanner().FindTypes<ServiceRegistry>();

            foreach (var type in registryTypes)
            {
                var registry = type.CreateInstance() as ServiceRegistry;
                if (registry == null) continue;
                foreach (var instance in registry.Instances)
                {
                    if (instance.Instance == null)
                    {
                        if (instance.ServiceName == null)
                        {
                            Register(instance.Service, instance.Implementation, instance.LifeCycle);
                            continue;
                        }

                        Register(instance.Service, instance.Implementation, instance.ServiceName, instance.LifeCycle);
                        continue;
                    }

                    container.RegisterInstance(instance.Service, instance.Instance);
                }
            }

            container.RegisterInstance(typeof(IObjectFactory), this);
        }

        public void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope)
        {
            var lifetime = lifeCycleToLifeTime[lifeCycle]();
            container.Register(serviceType, implementationType, lifetime);
        }

        public void Register(Type serviceType, Type implementationType, string serviceName, LifeCycle lifeCycle = LifeCycle.PerScope)
        {
            var lifetime = lifeCycleToLifeTime[lifeCycle]();
            container.Register(serviceType, implementationType, serviceName, lifetime);
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            container.RegisterInstance(serviceType, instance);
        }

        public void BeginScope()
        {
            scope = container.BeginScope();
        }

        public void EndScope()
        {
            scope.Dispose();
            scope = null;
        }

        private readonly IDictionary<LifeCycle, Func<ILifetime>> lifeCycleToLifeTime = new Dictionary
            <LifeCycle, Func<ILifetime>>
        {
            {LifeCycle.PerScope, () => new PerScopeLifetime()},
            {LifeCycle.Singleton, () => new PerContainerLifetime()},
            {LifeCycle.Unique, () => new PerRequestLifeTime()}
        };
    }
}