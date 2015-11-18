using System;
using System.Collections.Generic;

namespace EzBus.ObjectFactory
{
    public interface IObjectFactory : IHandleScopes
    {
        object GetInstance(Type type);
        T GetInstance<T>() where T : class;
        IEnumerable<T> GetInstances<T>() where T : class;
        void Initialize();
    }
}