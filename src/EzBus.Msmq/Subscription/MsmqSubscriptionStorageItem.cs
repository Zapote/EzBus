using System;

namespace EzBus.Msmq.Subscription
{
    [Serializable]
    public class MsmqSubscriptionStorageItem
    {
        public string Endpoint { get; set; }
        public string MessageType { get; set; }

        public override string ToString()
        {
            return $"Endpoint: {Endpoint}, MessageType: {MessageType}";
        }
    }
}