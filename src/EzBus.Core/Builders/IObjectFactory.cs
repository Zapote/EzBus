using System;

namespace EzBus.Core.Builders
{
    public interface IObjectFactory
    {
        object CreateInstance(Type type);
    }
}