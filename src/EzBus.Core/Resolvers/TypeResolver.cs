using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Utils;
using AssemblyScanner = EzBus.Core.Utils.AssemblyScanner;

namespace EzBus.Core.Resolvers
{
    public class TypeResolver
    {
        private static readonly IDictionary<Type, Type> resolvedTypes = new Dictionary<Type, Type>();
        private static readonly AssemblyScanner assemblyScanner = new AssemblyScanner();

        public static Type GetType<TInterface>()
        {
            var typeToResolve = typeof(TInterface);

            if (resolvedTypes.ContainsKey(typeToResolve))
            {
                return resolvedTypes[typeToResolve];
            }

            var types = assemblyScanner.FindTypes<TInterface>();
            var resolvedType = types.All(x => x.IsLocal()) ? types.Last() : types.Last(x => !x.IsLocal());

            resolvedTypes.Add(typeof(TInterface), resolvedType);

            return resolvedType;
        }
    }
}