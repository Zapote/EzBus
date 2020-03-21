using System;
using EzBus.Utils;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class SubscriptionManager : ISubscriptionManager
    {
        private readonly IAddressConfig busConfig;
        private readonly IChannelFactory channelFactory;

        public SubscriptionManager(IChannelFactory channelFactory, IAddressConfig busConfig)
        {
            this.channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Subscribe(string address, string messageName)
        {
            try
            {
                var channel = channelFactory.GetChannel();
                var queue = busConfig.Address.ToLower();
                var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

               // log.Info($"Subscribing to endpoint '{address}'. Routingkey '{routingKey}'");

                channel.QueueBind(queue, address, routingKey);
            }
            catch (Exception ex)
            {
               // log.Error($"Failed to subscribe to endpoint {address}", ex);
            }
        }

        public void Unsubscribe(string endpoint, string messageName)
        {
            try
            {
                endpoint = endpoint.ToLower();

                var channel = channelFactory.GetChannel();
                var queue = busConfig.Address.ToLower();
                var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

               // log.Info($"Unsubscribing from endpoint '{endpoint}'. Routingkey '{routingKey}'");

                channel.QueueUnbind(queue, endpoint, routingKey);
            }
            catch (Exception ex)
            {
               // log.Error($"Failed to unsubscribe to endpoint {endpoint}", ex);
            }
        }
    }
}
