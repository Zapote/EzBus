using System;
using System.Linq;
using EzBus.Core.Utils;

namespace EzBus.Core.Resolvers
{
    public abstract class ResolverBase<TInterface, TDefault>
        where TInterface : class
        where TDefault : TInterface
    {
        protected Type resolvedType;

        protected ResolverBase()
        {
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyScanner.FindTypes<TInterface>();
            if (types.Length <= 1 && types[0].IsLocal())
            {
                resolvedType = typeof(TDefault);
            }

            resolvedType = types.Last(x => !x.IsLocal());
        }
        protected TInterface GetInstance()
        {
            return resolvedType.CreateInstance() as TInterface;
        }
    }
}