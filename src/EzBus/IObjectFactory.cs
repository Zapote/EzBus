using System;

namespace EzBus
{
    public interface IObjectFactory
    {
        object CreateInstance(Type type);
        void Initialize();
        void BeginScope();
        void EndScope();
        void Register<TService, TImplementation>(LifeCycle lifeCycle = LifeCycle.Default) where TImplementation : TService;
        void Register<TService>(Type implementationType, LifeCycle lifeCycle = LifeCycle.Default);
        void Register<TService>(TService instance, LifeCycle lifeCycle = LifeCycle.Default);
        void Register(Type serviceType, Type implementationType, LifeCycle lifeCycle = LifeCycle.Default);
    }
}