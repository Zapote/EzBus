using System.Collections.Generic;
using System.Linq;
using EzBus.Core.Routing;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageRouting : IMessageRouting
    {
        private readonly List<KeyValuePair<string, string>> routing = new List<KeyValuePair<string, string>>();

        public string GetRoute(string @namespace, string messageType)
        {
            return routing.First(x => x.Key == @namespace + messageType).Value;
        }

        public void AddRoute(string assemblyName, string messageType, string endpoint)
        {
            routing.Add(new KeyValuePair<string, string>(assemblyName + messageType, endpoint));
        }
    }
}