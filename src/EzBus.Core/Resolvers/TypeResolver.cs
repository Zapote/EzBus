using System;
using System.Collections.Concurrent;
using System.Linq;
using EzBus.Utils;
using AssemblyScanner = EzBus.Core.Utils.AssemblyScanner;

namespace EzBus.Core.Resolvers
{
    public class TypeResolver
    {
        private static readonly ConcurrentDictionary<Type, Type> resolvedTypes = new ConcurrentDictionary<Type, Type>();
        private static readonly AssemblyScanner assemblyScanner = new AssemblyScanner();

        public static Type GetType<TInterface>()
        {
            var typeToResolve = typeof(TInterface);

            if (resolvedTypes.ContainsKey(typeToResolve)) return resolvedTypes[typeToResolve];

            var types = assemblyScanner.FindTypes<TInterface>();
            var resolvedType = types.All(x => x.IsLocal()) ? types.Last() : types.Last(x => !x.IsLocal());

            resolvedTypes.AddOrUpdate(typeToResolve, resolvedType, (key, oldValue) => oldValue);

            return resolvedType;
        }
    }
}