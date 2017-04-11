using System;
using System.Collections.Generic;
using EzBus.ObjectFactory;
using EzBus.Utils;
using StructureMap;
using StructureMap.Pipeline;

namespace EzBus.StructureMap
{
    public class StructureMapObjectFactory : IObjectFactory
    {
        private static IContainer container = new Container();

        public void BeginScope()
        {
            container.CreateChildContainer();
        }

        public void EndScope()
        {
            throw new NotImplementedException();
        }

        public object GetInstance(Type type)
        {
            return container.GetInstance(type);
        }

        public T GetInstance<T>() where T : class
        {
            return container.GetInstance(typeof(T)) as T;
        }

        public IEnumerable<T> GetInstances<T>() where T : class
        {
            return (IEnumerable<T>)container.GetAllInstances(typeof(T));
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

                    RegisterInstance(instance.Service, instance.Instance);
                }
            }

            RegisterInstance(typeof(IObjectFactory), this);
        }

        public void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope)
        {
            container.Configure(x => x.For(serviceType, lifecycles[lifeCycle]()).Use(implementationType));
        }

        public void Register(Type serviceType, Type implementationType, string serviceName, LifeCycle lifeCycle = LifeCycle.PerScope)
        {
            container.Configure(x => x.For(serviceType, lifecycles[lifeCycle]()).Use(implementationType).Named(serviceName));
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            container.Configure(x => x.For(serviceType).Use(instance));
        }

        private readonly IDictionary<LifeCycle, Func<ILifecycle>> lifecycles = new Dictionary
           <LifeCycle, Func<ILifecycle>>
        {
            {LifeCycle.PerScope, () => new ContainerLifecycle() },
            {LifeCycle.Singleton, () => new SingletonLifecycle() },
            {LifeCycle.Unique, () => new UniquePerRequestLifecycle() }
        };
    }
}
