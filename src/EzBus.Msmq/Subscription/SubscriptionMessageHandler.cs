using System;

namespace EzBus.Msmq.Subscription
{
    public class SubscriptionMessageHandler : IHandle<SubscriptionMessage>
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public SubscriptionMessageHandler(ISubscriptionStorage subscriptionStorage)
        {
            this.subscriptionStorage = subscriptionStorage ?? throw new ArgumentNullException(nameof(subscriptionStorage));
        }

        public void Handle(SubscriptionMessage message)
        {
            subscriptionStorage.Store(message.Endpoint, null);
        }
    }
}