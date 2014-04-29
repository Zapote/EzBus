using System;
using System.Collections.Generic;

namespace EzBus.Core
{
    public interface ISubscriptionStorage
    {
        void Store(Subscriber subscriber);
        IEnumerable<Subscriber> GetSubscribers(Type messageType);
    }
}