using System;
using System.Collections.Generic;

namespace EzBus.ObjectFactory
{
    public interface IObjectFactory : IHandleScopes
    {
        object GetInstance(Type type);
        T GetInstance<T>() where T : class;
        IEnumerable<object> GetInstances(Type type);
        IEnumerable<T> GetInstances<T>() where T : class;
        void Initialize();
        void Register<TService, TImplementation>(LifeCycle lifeCycle = LifeCycle.PerScope) where TImplementation : TService;
        void Register<TService>(Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope);
        void Register<TService>(TService instance, LifeCycle lifeCycle = LifeCycle.PerScope);
        void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope);
    }
}