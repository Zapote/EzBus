using System;
using System.Linq;
using EzBus.Core.Utils;

namespace EzBus.Core.Resolvers
{
    public abstract class ResolverBase<TInterface>
        where TInterface : class
    {
        protected Type resolvedType;

        protected ResolverBase()
        {
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyScanner.FindTypes<TInterface>();
            resolvedType = types.All(x => x.IsLocal()) ? types.Last() : types.Last(x => !x.IsLocal());
        }

        protected TInterface GetInstance()
        {
            return resolvedType.CreateInstance() as TInterface;
        }
    }
}