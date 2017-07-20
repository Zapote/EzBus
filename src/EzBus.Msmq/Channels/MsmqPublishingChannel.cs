using System;
using System.Linq;
using EzBus.Msmq.Subscription;

namespace EzBus.Msmq.Channels
{
    public class MsmqPublishingChannel : MsmqSendingChannel, IPublishingChannel
    {
        private readonly ISubscriptionStorage subscriptionStorage;

        public MsmqPublishingChannel(ISubscriptionStorage subscriptionStorage)
        {
            this.subscriptionStorage = subscriptionStorage ?? throw new ArgumentNullException(nameof(subscriptionStorage));
        }

        public void Publish(ChannelMessage channelMessage)
        {
            var messageName = channelMessage.GetHeader(MessageHeaders.MessageName);
            var endpoints = subscriptionStorage.GetSubscribers(messageName);

            foreach (var destination in endpoints.Select(EndpointAddress.Parse))
            {
                Send(destination, channelMessage);
            }
        }
    }
}