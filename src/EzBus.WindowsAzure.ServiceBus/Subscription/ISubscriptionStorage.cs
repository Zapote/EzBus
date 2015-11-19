using System;
using System.Collections.Generic;

namespace EzBus.WindowsAzure.ServiceBus.Subscription
{
    public interface ISubscriptionStorage
    {
        void Store(string endpoint, Type messageType);
        IEnumerable<string> GetSubscribersEndpoints(string messageType);
    }
}