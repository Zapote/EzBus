using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core
{
    public class InMemorySubscriptionStorage : ISubscriptionStorage
    {
        private readonly static List<Subscription> subscriptions = new List<Subscription>();

        public void Initialize(string endpointName) { }

        public void Store(string endpoint, Type messageType)
        {
            var existing = subscriptions.FirstOrDefault(x => x.Endpoint == endpoint);

            if (existing != null)
            {
                subscriptions.Remove(existing);
            }

            subscriptions.Add(new Subscription { Endpoint = endpoint, MessageType = messageType });
        }

        public IEnumerable<string> GetSubscribersEndpoints(Type messageType)
        {
            return subscriptions.Where(x => x.MessageType == null || x.MessageType == messageType).Select(x => x.Endpoint);
        }
    }
}