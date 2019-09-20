using System;

namespace EzBus.Msmq.Subscription
{
    public class UnsubscribeMessageHandler : IHandle<UnsubscribeMessage>
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public UnsubscribeMessageHandler(ISubscriptionStorage subscriptionStorage)
        {
            this.subscriptionStorage = subscriptionStorage ?? throw new ArgumentNullException(nameof(subscriptionStorage));
        }

        public void Handle(UnsubscribeMessage message)
        {
            subscriptionStorage.Remove(message.Endpoint, message.MessageName);
        }
    }
}