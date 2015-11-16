using System;
using System.Linq;
using EzBus.Msmq.Subscription;

namespace EzBus.Msmq.Channels
{
    public class MsmqPublishingChannel : MsmqSendingChannel, IPublishingChannel
    {
        private readonly IMsmqSubscriptionStorage msmqSubscriptionStorage;

        public MsmqPublishingChannel(IMsmqSubscriptionStorage msmqSubscriptionStorage)
        {
            if (msmqSubscriptionStorage == null) throw new ArgumentNullException(nameof(msmqSubscriptionStorage));
            this.msmqSubscriptionStorage = msmqSubscriptionStorage;
        }

        public void Publish(ChannelMessage channelMessage)
        {
            var messageType = channelMessage.GetHeader(MessageHeaders.MessageType);
            var endpoints = msmqSubscriptionStorage.GetSubscribers(messageType);

            foreach (var destination in endpoints.Select(EndpointAddress.Parse))
            {
                Send(destination, channelMessage);
            }
        }
    }
}