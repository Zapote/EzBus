using System;

namespace EzBus.Core.Utils
{
    public static class TypeExtensions
    {
        public static object CreateInstance(this Type t)
        {
            return Activator.CreateInstance(t);
        }

        public static bool IsLocal(this Type t)
        {
            return t.Namespace != null && t.Namespace.Equals("EzBus.Core");
        }

        public static string GetAssemblyName(this object obj)
        {
            return obj.GetType().Assembly.GetName().Name;
        }
    }
}