namespace EzBus.WindowsAzure.ServiceBus
{
    public class SubscriptionMessageHandler : IHandle<SubscriptionMessage>
    {
        private static readonly AzureTableSubscriptionStorage subscriptionStorage = new AzureTableSubscriptionStorage();

        public void Handle(SubscriptionMessage message)
        {
            subscriptionStorage.Store(message.Endpoint, null);
        }
    }
}