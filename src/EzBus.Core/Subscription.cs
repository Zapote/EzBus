using System;

namespace EzBus.Core
{
    public class Subscription
    {
        public string Endpoint { get; set; }
        public Type MessageType { get; set; }
    }
}