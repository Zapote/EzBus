using System.Collections.Generic;

namespace EzBus.Msmq.Subscription
{
    public interface IMsmqSubscriptionStorage
    {
        void Store(string endpoint, string messageType);
        IEnumerable<string> GetSubscribers(string messageType);
        void Initialize();
    }
}