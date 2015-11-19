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
            if (subscriptionStorage == null) throw new ArgumentNullException(nameof(subscriptionStorage));
            this.subscriptionStorage = subscriptionStorage;
        }

        public void Publish(ChannelMessage channelMessage)
        {
            var messageType = channelMessage.GetHeader(MessageHeaders.MessageType);
            var endpoints = subscriptionStorage.GetSubscribers(messageType);

            foreach (var destination in endpoints.Select(EndpointAddress.Parse))
            {
                Send(destination, channelMessage);
            }
        }
    }
}