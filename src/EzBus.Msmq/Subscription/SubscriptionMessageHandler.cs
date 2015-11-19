using System;

namespace EzBus.Msmq.Subscription
{
    public class SubscriptionMessageHandler : IHandle<SubscriptionMessage>
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public SubscriptionMessageHandler(ISubscriptionStorage subscriptionStorage)
        {
            if (subscriptionStorage == null) throw new ArgumentNullException(nameof(subscriptionStorage));
            this.subscriptionStorage = subscriptionStorage;
        }

        public void Handle(SubscriptionMessage message)
        {
            subscriptionStorage.Store(message.Endpoint, null);
        }
    }
}