using System;

namespace EzBus.Msmq.Subscription
{
    public class SubscribeMessageHandler : IHandle<SubscribeMessage>
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public SubscribeMessageHandler(ISubscriptionStorage subscriptionStorage)
        {
            this.subscriptionStorage = subscriptionStorage ?? throw new ArgumentNullException(nameof(subscriptionStorage));
        }

        public void Handle(SubscribeMessage message)
        {
            subscriptionStorage.Store(message.Endpoint, message.MessageName);
        }
    }
}