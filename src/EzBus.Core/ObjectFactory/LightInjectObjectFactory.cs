using System;
using System.Collections.Generic;
using EzBus.Core.ObjectFactory.LightInject;
using EzBus.Core.Utils;
using EzBus.ObjectFactory;

namespace EzBus.Core.ObjectFactory
{
    public class LightInjectObjectFactory : IObjectFactory
    {
        private readonly IServiceContainer container = new ServiceContainer();
        private Scope scope;

        public object GetInstance(Type type)
        {
            return container.GetInstance(type);
        }

        public T GetInstance<T>() where T : class
        {
            return container.GetInstance<T>();
        }

        public IEnumerable<object> GetInstances(Type type)
        {
            return container.GetAllInstances(type);
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
                        Register(instance.Service, instance.Implementation, instance.LifeCycle);
                        continue;
                    }

                    container.RegisterInstance(instance.Service, instance.Instance);
                }
            }
        }

        public void Register<TService, TImplementation>(LifeCycle lifeCycle = LifeCycle.PerScope) where TImplementation : TService
        {
            Register(typeof(TService), typeof(TImplementation), lifeCycle);
        }

        public void Register<TService>(Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope)
        {
            Register(typeof(TService), implementationType, lifeCycle);
        }

        public void Register<TService>(TService instance, LifeCycle lifeCycle = LifeCycle.PerScope)
        {
            var lifetime = lifeCycleToLifeTime[lifeCycle]();
            container.Register(f => instance, lifetime);
        }

        public void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope)
        {
            var lifetime = lifeCycleToLifeTime[lifeCycle]();
            container.Register(serviceType, implementationType, lifetime);
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