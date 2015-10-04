namespace EzBus.Msmq
{
    public class SubscriptionMessageHandler : IHandle<SubscriptionMessage>
    {
        private readonly MsmqSubscriptionStorage subscriptionStorage;

        public SubscriptionMessageHandler()
        {
            subscriptionStorage = new MsmqSubscriptionStorage();
        }

        public void Handle(SubscriptionMessage message)
        {
            subscriptionStorage.Store(message.Endpoint, null);
        }
    }
}