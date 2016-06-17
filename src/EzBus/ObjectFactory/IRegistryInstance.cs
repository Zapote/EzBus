using System;

namespace EzBus.ObjectFactory
{
    public interface IRegistryInstance : ILifeCycle, ILifeCycleConfiguration
    {
        Type Implementation { get; }
        Type Service { get; }
        string ServiceName { get; }
        object Instance { get; }
        LifeCycle LifeCycle { get; }
    }
}