using System;

namespace EzBus.Msmq
{
    [Serializable]
    public class MsmqSubscriptionStorageItem
    {
        public string Endpoint { get; set; }
        public Type MessageType { get; set; }

        public override string ToString()
        {
            return string.Format("Endpoint: {0}, MessageType: {1}", Endpoint, MessageType);
        }
    }
}