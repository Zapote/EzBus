using System;

namespace EzBus.Core.Builders
{
    public class RegistryInstance : IHaveLifeCycle, IConfigureLifeCycle
    {
        public RegistryInstance(Type service, Type implementation)
        {
            Service = service;
            Implementation = implementation;
        }

        public Type Implementation { get; private set; }
        public Type Service { get; private set; }
        public LifeCycle LifeCycle { get; private set; }

        public void Singleton()
        {
            LifeCycle = LifeCycle.Singleton;
        }

        public void Unique()
        {
            LifeCycle = LifeCycle.Unique;
        }

        public IHaveLifeCycle As
        {
            get { return this; }
        }
    }
}