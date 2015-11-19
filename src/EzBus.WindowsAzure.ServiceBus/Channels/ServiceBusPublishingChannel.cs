using System;
using System.Linq;
using EzBus.WindowsAzure.ServiceBus.Subscription;

namespace EzBus.WindowsAzure.ServiceBus.Channels
{
    public class ServiceBusPublishingChannel : ServiceBusSendingChannel, IPublishingChannel
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public ServiceBusPublishingChannel(ISubscriptionStorage subscriptionStorage)
        {
            if (subscriptionStorage == null) throw new ArgumentNullException(nameof(subscriptionStorage));
            this.subscriptionStorage = subscriptionStorage;
        }

        public void Publish(ChannelMessage channelMessage)
        {
            var messageType = channelMessage.GetHeader(MessageHeaders.MessageType);
            var endpoints = subscriptionStorage.GetSubscribersEndpoints(messageType);

            foreach (var endpoint in endpoints.Select(EndpointAddress.Parse))
            {
                Send(endpoint, channelMessage);
            }
        }
    }
}