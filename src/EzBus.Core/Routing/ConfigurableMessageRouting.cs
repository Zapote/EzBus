using System.Collections.Generic;
using EzBus.Core.Config;

namespace EzBus.Core.Routing
{
    public class ConfigurableMessageRouting : IMessageRouting
    {
        private readonly IDictionary<int, string> routingTable = new Dictionary<int, string>();

        public ConfigurableMessageRouting()
        {
            var destinations = DestinationSection.Section.Destinations;

            foreach (DestinationElement destination in destinations)
            {
                var key = CreateKey(destination.Assembly, destination.Message);

                if (routingTable.ContainsKey(key))
                {
                    throw new MessageRoutingException($"Multiple setup for {destination}");
                }

                routingTable.Add(key, destination.Endpoint);
            }
        }

        private static int CreateKey(string assembly, string messageType)
        {
            return string.IsNullOrWhiteSpace(messageType) ?
                assembly.GetHashCode() : $"{assembly}::{messageType}".GetHashCode();
        }

        public string GetRoute(string asssemblyName, string messageType)
        {
            var specificKey = CreateKey(asssemblyName, messageType);
            if (routingTable.ContainsKey(specificKey)) return routingTable[specificKey];

            var genericKey = CreateKey(asssemblyName, null);
            if (routingTable.ContainsKey(genericKey)) return routingTable[genericKey];

            throw new DestinationMissingException("No destination exists for " + messageType);
        }
    }
}
