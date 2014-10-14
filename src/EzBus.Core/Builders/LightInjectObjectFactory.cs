using System;
using System.Collections.Generic;
using EzBus.Core.Builders.LightInject;

namespace EzBus.Core.Builders
{
    public class LightInjectObjectFactory : IObjectFactory
    {
        private readonly IServiceContainer container = new ServiceContainer();
        private Scope scope;

        public object CreateInstance(Type type)
        {
            return container.GetInstance(type);
        }

        public void Initialize()
        {
            var registryTypes = new AssemblyScanner().FindTypes<ServiceRegistry>();

            foreach (var type in registryTypes)
            {
                var registry = type.CreateInstance() as ServiceRegistry;
                if (registry == null) continue;
                foreach (var instance in registry.Instances)
                {
                    Register(instance.Service, instance.Implementation, instance.LifeCycle);
                }
            }
        }

        public void BeginScope()
        {
            scope = container.BeginScope();
        }

        public void EndScope()
        {
            scope.Dispose();
        }

        public void Register<TService, TImplementation>(LifeCycle lifeCycle = LifeCycle.Default) where TImplementation : TService
        {
            Register(typeof(TService), typeof(TImplementation), lifeCycle);
        }

        public void Register<TService>(Type implementationType, LifeCycle lifeCycle = LifeCycle.Default)
        {
            Register(typeof(TService), implementationType, lifeCycle);
        }

        public void Register<TService>(TService instance, LifeCycle lifeCycle = LifeCycle.Default)
        {
            var lifetime = lifeCycleToLifeTime[lifeCycle]();
            container.Register(f => instance, lifetime);
        }

        public void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.Default)
        {
            var lifetime = lifeCycleToLifeTime[lifeCycle]();
            container.Register(serviceType, implementationType, lifetime);
        }

        private readonly IDictionary<LifeCycle, Func<ILifetime>> lifeCycleToLifeTime = new Dictionary<LifeCycle, Func<ILifetime>>
        {
            {LifeCycle.Default, () => new PerScopeLifetime()},
            {LifeCycle.Singleton, () => new PerContainerLifetime()},
            {LifeCycle.Unique,  () => new PerRequestLifeTime()}
        };
    }
}