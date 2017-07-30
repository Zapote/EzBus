using System;
using System.Collections.Generic;

namespace EzBus.ObjectFactory
{
    public interface IObjectFactory : IHandleScopes
    {
        object GetInstance(Type type);
        T GetInstance<T>() where T : class;
        IEnumerable<T> GetInstances<T>() where T : class;
        IEnumerable<object> GetInstances(Type type);
        object CreateInstance(Type type);
        T CreateInstance<T>() where T : class;
        void Initialize();
        void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope);
        void Register(Type serviceType, Type implementationType, string serviceName, LifeCycle lifeCycle = LifeCycle.PerScope);
        void RegisterInstance(Type serviceType, object instance);
    }
}