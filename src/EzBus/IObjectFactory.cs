using System;

namespace EzBus
{
    public interface IObjectFactory
    {
        object GetInstance(Type type);
        T GetInstance<T>() where T : class;
        void Initialize();
        void BeginScope();
        void EndScope();
        void Register<TService, TImplementation>(LifeCycle lifeCycle = LifeCycle.PerScope) where TImplementation : TService;
        void Register<TService>(Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope);
        void Register<TService>(TService instance, LifeCycle lifeCycle = LifeCycle.PerScope);
        void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.PerScope);
    }
}