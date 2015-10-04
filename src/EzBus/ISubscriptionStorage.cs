using System;
using System.Collections.Generic;

namespace EzBus
{
    [Obsolete("Should be in each transport instead")]
    public interface ISubscriptionStorage
    {
        void Initialize(string endpointName);
        void Store(string endpoint, Type messageType);
        IEnumerable<string> GetSubscribersEndpoints(Type messageType);
    }
}