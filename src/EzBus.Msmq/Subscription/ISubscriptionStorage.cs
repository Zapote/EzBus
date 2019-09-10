using System.Collections.Generic;

namespace EzBus.Msmq.Subscription
{
    public interface ISubscriptionStorage
    {
        void Store(string endpoint, string messageName);
        void Remove(string endpoint, string messageName);
        IEnumerable<string> GetSubscribers(string messageName);
        void Initialize();
    }
}