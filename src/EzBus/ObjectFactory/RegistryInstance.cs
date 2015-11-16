using System;

namespace EzBus.ObjectFactory
{
    public class RegistryInstance : IRegistryInstance
    {
        public RegistryInstance(Type service, Type implementation)
        {
            Service = service;
            Implementation = implementation;
        }

        public RegistryInstance(Type service, object instance)
        {
            Service = service;
            Implementation = instance.GetType();
            Instance = instance;
        }

        public Type Implementation { get; }
        public Type Service { get; }
        public object Instance { get; }
        public LifeCycle LifeCycle { get; private set; }

        public void Singleton()
        {
            LifeCycle = LifeCycle.Singleton;
        }

        public void Unique()
        {
            LifeCycle = LifeCycle.Unique;
        }

        public ILifeCycle As => this;
    }
}