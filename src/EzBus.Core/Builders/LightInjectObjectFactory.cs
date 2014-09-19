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
            return container.TryGetInstance(type);
        }

        public void Initialize()
        {
            var registryTypes = new AssemblyScanner().FindTypes<ServiceRegistry>();

            foreach (var type in registryTypes)
            {
                var registry = container.GetInstance(type) as ServiceRegistry;
                if (registry == null) continue;
                foreach (var instance in registry.Instances)
                {
                    var lifetime = lifeCycleToLifeTime[instance.LifeCycle];
                    container.Register(instance.Service, instance.Implementation, lifetime);
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

        private readonly IDictionary<LifeCycle, ILifetime> lifeCycleToLifeTime = new Dictionary<LifeCycle, ILifetime>
        {
            {LifeCycle.Default, new PerScopeLifetime()},
            {LifeCycle.Singleton, new PerContainerLifetime()},
            {LifeCycle.Unique, new PerRequestLifeTime()}
        };
    }
}