using System;
using System.Collections.Generic;

namespace EzBus
{
    public interface ISubscriptionStorage
    {
        void Initialize(EndpointAddress inputAddress);
        void Store(string endpoint, Type messageType);
        IEnumerable<string> GetSubscribersEndpoints(Type messageType);
    }
}