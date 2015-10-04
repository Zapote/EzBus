﻿using System;
using System.Linq;
using EzBus.Core.Utils;

namespace EzBus.Core.Resolvers
{
    internal class SubscriptionStorageResolver : ResolverBase<ISubscriptionStorage>
    {
        private static readonly SubscriptionStorageResolver instance = new SubscriptionStorageResolver();
        private static ISubscriptionStorage storage;

        public static ISubscriptionStorage GetSubscriptionStorage()
        {
            return storage ?? (storage = instance.GetInstance());
        }
    }
}