using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EzBus.Utils
{
    public static class TypeExtensions
    {
        public static object CreateInstance(this Type t)
        {
            return Activator.CreateInstance(t);
        }

        public static bool IsLocal(this Type t)
        {
            if (t.Namespace == null) return false;
            var isLocalNamespace = t.Namespace.Equals("EzBus") ||
                                   t.Namespace.StartsWith("EzBus.Core");
            return isLocalNamespace;
        }

        public static string GetAssemblyName(this object obj)
        {
            return obj.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        public static bool IsCollection(this Type type)
        {
            return
                (
                    type != typeof(string) &&
                    type.GetTypeInfo().GetInterfaces().Any(t => t.Name == typeof(IEnumerable).Name)
                );
        }

        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        public static bool IsClass(this Type type)
        {
            return type.GetTypeInfo().IsClass && type != typeof(string);
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType || type == typeof(string);
        }

        public static IList CreateGenericList(this Type type)
        {
            var listType = typeof(List<>);
            var concreteType = listType.MakeGenericType(type);
            var list = (IList)Activator.CreateInstance(concreteType);
            return list;
        }

        public static bool ImplementsInterface<T>(this Type type)
        {
            var interfaces = type.GetTypeInfo().GetInterfaces();
            return interfaces.Any(t => t == typeof(T));
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            var interfaces = type.GetTypeInfo().GetInterfaces();
            return interfaces.Any(t => t == interfaceType);
        }

        public static Type GetInterface(this Type type, string name)
        {
            return type.GetTypeInfo().GetInterface(name);
        }

        public static MethodInfo GetMethod(this Type type, string name, Type[] types)
        {
            return type.GetTypeInfo().GetMethod(name, types);
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments();
        }
    }
}