using System;
using System.Linq;

namespace EzBus.Core.Resolvers
{
    internal class SubscriptionStorageResolver
    {
        private static Type subscriptionsStorageType = typeof(InMemorySubscriptionStorage);
        private static ISubscriptionStorage instance;
        static SubscriptionStorageResolver()
        {
            ResolveTypes();
        }

        private static void ResolveTypes()
        {
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyScanner.FindTypes<ISubscriptionStorage>();
            if (types.Length <= 1 && types[0].IsLocal())
            {
                return;
            }

            subscriptionsStorageType = types.Last(x => !x.IsLocal());
        }

        public static ISubscriptionStorage GetSubscriptionStorage()
        {
            return instance ?? (instance = subscriptionsStorageType.CreateInstance() as ISubscriptionStorage);
        }
    }
}