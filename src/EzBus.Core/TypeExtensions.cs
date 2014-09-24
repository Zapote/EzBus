using System;

namespace EzBus.Core
{
    public static class TypeExtensions
    {
        public static object CreateInstance(this Type t)
        {
            return Activator.CreateInstance(t);
        }
    }
}