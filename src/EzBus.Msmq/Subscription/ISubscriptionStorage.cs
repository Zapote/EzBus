using System.Collections.Generic;

namespace EzBus.Msmq.Subscription
{
    public interface ISubscriptionStorage
    {
        void Store(string endpoint, string messageType);
        IEnumerable<string> GetSubscribers(string messageType);
    }
}