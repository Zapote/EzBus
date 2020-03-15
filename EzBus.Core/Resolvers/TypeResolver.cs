using System;
using System.Collections.Concurrent;
using System.Linq;
using EzBus.Core.Utils;
using EzBus.Utils;


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

            var resolvedType = ResolveType(typeToResolve);
            resolvedTypes.AddOrUpdate(typeToResolve, resolvedType, (key, oldValue) => oldValue);

            return resolvedType;
        }

        private static Type ResolveType(Type type)
        {
            var types = assemblyScanner.FindTypes(type);
            var resolvedType = types.All(x => x.IsLocal()) ? types.LastOrDefault() : types.LastOrDefault(x => !x.IsLocal());

            if (resolvedType != null) return resolvedType;

            throw new Exception($"Unable to resolve type: {type}");
        }
    }
}