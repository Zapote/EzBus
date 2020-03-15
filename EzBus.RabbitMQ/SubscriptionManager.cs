using System;
using EzBus.Logging;
using EzBus.Utils;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class SubscriptionManager : ISubscriptionManager
    {
        private static readonly ILogger log = LogManager.GetLogger<SubscriptionManager>();
        private readonly IBusConfig busConfig;
        private readonly IChannelFactory channelFactory;

        public SubscriptionManager(IChannelFactory channelFactory, IBusConfig busConfig)
        {
            this.channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Subscribe(string endpoint)
        {
            Subscribe(endpoint, string.Empty);
        }

        public void Subscribe(string endpoint, string messageName)
        {
            try
            {
                endpoint = endpoint.ToLower();

                var channel = channelFactory.GetChannel();
                var queue = busConfig.EndpointName.ToLower();
                var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

                log.Info($"Subscribing to endpoint '{endpoint}'. Routingkey '{routingKey}'");

                channel.QueueBind(queue, endpoint, routingKey);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to subscribe to endpoint {endpoint}", ex);
            }
        }

        public void Unsubscribe(string endpoint)
        {
            Unsubscribe(endpoint, string.Empty);
        }

        public void Unsubscribe(string endpoint, string messageName)
        {
            try
            {
                endpoint = endpoint.ToLower();

                var channel = channelFactory.GetChannel();
                var queue = busConfig.EndpointName.ToLower();
                var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

                log.Info($"Unsubscribing from endpoint '{endpoint}'. Routingkey '{routingKey}'");

                channel.QueueUnbind(queue, endpoint, routingKey);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to unsubscribe to endpoint {endpoint}", ex);
            }
        }
    }
}
