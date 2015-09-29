using System;
using EzBus.Core.Resolvers;

namespace EzBus.Core
{
    public class SubscriptionMessageHandler : IHandle<SubscriptionMessage>
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public SubscriptionMessageHandler()
        {
            subscriptionStorage = SubscriptionStorageResolver.GetSubscriptionStorage();
        }
        
        public void Handle(SubscriptionMessage message)
        {
            subscriptionStorage.Store(message.Endpoint, null);
        }
    }
}