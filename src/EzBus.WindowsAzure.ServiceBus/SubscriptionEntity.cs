using Microsoft.WindowsAzure.Storage.Table;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class SubscriptionEntity : TableEntity
    {
        public SubscriptionEntity() { }

        public SubscriptionEntity(string endpoint, string messageType)
        {
            Endpoint = endpoint;
            MessageType = messageType;

            PartitionKey = endpoint;
            RowKey = messageType;
        }

        public string Endpoint { get; set; }
        public string MessageType { get; set; }
    }
}