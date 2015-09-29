using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core.Subscription
{
    public class InMemorySubscriptionStorage : ISubscriptionStorage
    {
        private readonly static List<Subscriber> subscribers = new List<Subscriber>();

        public void Initialize(string endpointName) { }

        public void Store(string endpoint, Type messageType)
        {
            var existing = subscribers.FirstOrDefault(x => x.Endpoint == endpoint);

            if (existing != null)
            {
                subscribers.Remove(existing);
            }

            subscribers.Add(new Subscriber { Endpoint = endpoint, MessageType = messageType });
        }

        public IEnumerable<string> GetSubscribersEndpoints(Type messageType)
        {
            return subscribers.Where(x => x.MessageType == null || x.MessageType == messageType).Select(x => x.Endpoint);
        }
    }
}