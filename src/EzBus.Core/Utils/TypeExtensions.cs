using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            return t.Namespace != null && t.Namespace.StartsWith("EzBus");
        }

        public static string GetAssemblyName(this object obj)
        {
            return obj.GetType().Assembly.GetName().Name;
        }

        public static bool IsCollection(this Type type)
        {
            var i = type.GetInterfaces();
            return
                (
                    type != typeof(string) &&
                    type.GetInterfaces().Any(t => t.Name == typeof(IEnumerable).Name)
                    );
        }

        public static bool IsClass(this Type type)
        {
            return type.IsClass && type != typeof(string);
        }

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        public static IList CreateGenericList(this Type type)
        {
            var listType = typeof(List<>);
            var concreteType = listType.MakeGenericType(type);
            var list = (IList)Activator.CreateInstance(concreteType);
            return list;
        }
    }
}