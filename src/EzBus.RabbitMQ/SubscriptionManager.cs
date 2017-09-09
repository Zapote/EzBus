using System;
using EzBus.Logging;
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
            try
            {
                endpoint = endpoint.ToLower();
                log.Info($"Subscribing to endpoint '{endpoint}'");
                var channel = channelFactory.GetChannel();
                var queue = busConfig.EndpointName.ToLower();
                channel.QueueBind(queue, endpoint, string.Empty);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to subscribe to endpoint {endpoint}", ex);
            }
        }
    }
}
