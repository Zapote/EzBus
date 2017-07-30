using System.Collections.Generic;
using EzBus.Core.Config;

namespace EzBus.Core.Routing
{
    public class ConfigurableMessageRouting : IMessageRouting
    {
        private readonly IDictionary<int, string> routingTable = new Dictionary<int, string>();

        public ConfigurableMessageRouting()
        {
            var config = EzBusConfig.GetConfig();//TODO: Inject
            var destinations = config.Destinations;

            if (destinations == null) return;

            foreach (var destination in destinations)
            {
                var key = CreateKey(destination.Namespace, destination.Message);

                if (routingTable.ContainsKey(key))
                {
                    throw new MessageRoutingException($"Multiple setup for {destination}");
                }

                routingTable.Add(key, destination.Endpoint);
            }
        }

        public string GetRoute(string @namespace, string messageType)
        {
            var specificKey = CreateKey(@namespace, messageType);
            if (routingTable.ContainsKey(specificKey)) return routingTable[specificKey];

            var genericKey = CreateKey(@namespace, null);
            if (routingTable.ContainsKey(genericKey)) return routingTable[genericKey];

            throw new DestinationMissingException("No destination exists for " + messageType);
        }

        private static int CreateKey(string @namespace, string messageType)
        {
            @namespace = @namespace.ToLower();
            messageType = messageType?.ToLower();

            return string.IsNullOrWhiteSpace(messageType) ?
                @namespace.ToLower().GetHashCode() : $"{@namespace}::{messageType}".GetHashCode();
        }
    }
}
