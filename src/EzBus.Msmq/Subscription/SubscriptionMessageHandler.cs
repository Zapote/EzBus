using System;

namespace EzBus.Msmq.Subscription
{
    public class SubscriptionMessageHandler : IHandle<SubscriptionMessage>
    {
        private readonly IMsmqSubscriptionStorage msmqSubscriptionStorage;

        public SubscriptionMessageHandler(IMsmqSubscriptionStorage msmqSubscriptionStorage)
        {
            if (msmqSubscriptionStorage == null) throw new ArgumentNullException(nameof(msmqSubscriptionStorage));
            this.msmqSubscriptionStorage = msmqSubscriptionStorage;
        }

        public void Handle(SubscriptionMessage message)
        {
            msmqSubscriptionStorage.Store(message.Endpoint, null);
        }
    }
}