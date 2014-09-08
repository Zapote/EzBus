using System;
using System.Collections.Generic;

namespace EzBus.Core
{
    public interface ISubscriptionStorage
    {
        void Store(string endpoint, Type messageType);
        IEnumerable<string> GetSubscribersEndpoints(Type messageType);
    }
}