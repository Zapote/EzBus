using System;

namespace EzBus.Core.Subscription
{
    public class Subscriber
    {
        public string Endpoint { get; set; }
        public Type MessageType { get; set; }
    }
}