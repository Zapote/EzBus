using System;

namespace EzBus
{
    public interface IObjectFactory
    {
        object CreateInstance(Type type);
        void Initialize();
        void BeginScope();
        void EndScope();
    }
}