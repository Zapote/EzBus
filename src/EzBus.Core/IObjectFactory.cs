using System;

namespace EzBus.Core
{
    public interface IObjectFactory
    {
        object CreateInstance(Type type);
    }
}