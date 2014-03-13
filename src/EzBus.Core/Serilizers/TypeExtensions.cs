using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core.Serilizers
{
    public static class TypeExtensions
    {
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