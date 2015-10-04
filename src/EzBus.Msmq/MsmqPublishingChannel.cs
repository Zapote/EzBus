using System.Linq;

namespace EzBus.Msmq
{
    public class MsmqPublishingChannel : IPublishingChannel
    {
        private readonly MsmqSubscriptionStorage subscriptionStorage = new MsmqSubscriptionStorage();

        public void Publish(ChannelMessage channelMessage)
        {
            var messageType = channelMessage.GetHeader(MessageHeaders.MessageType);
            var endpoints = subscriptionStorage.GetSubscribersEndpoints(messageType);
            foreach (var destination in endpoints.Select(EndpointAddress.Parse))
            {
                MsmqUtilities.WriteMessage(destination, channelMessage);
            }
        }
    }
}