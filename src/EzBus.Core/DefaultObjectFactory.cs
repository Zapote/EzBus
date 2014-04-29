using System;

namespace EzBus.Core
{
    public class DefaultObjectFactory : IObjectFactory
    {
        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}