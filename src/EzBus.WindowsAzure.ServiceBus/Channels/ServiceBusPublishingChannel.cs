using System.Linq;

namespace EzBus.WindowsAzure.ServiceBus.Channels
{
    public class ServiceBusPublishingChannel : ServiceBusSendingChannel, IPublishingChannel
    {
       // private readonly AzureTableSubscriptionStorage subscriptionStorage = new AzureTableSubscriptionStorage();

        public void Publish(ChannelMessage channelMessage)
        {
            //var messageType = channelMessage.GetHeader(MessageHeaders.MessageType);
            //var endpoints = subscriptionStorage.GetSubscribersEndpoints(messageType);

            //foreach (var endpoint in endpoints.Select(EndpointAddress.Parse))
            //{
            //    Send(endpoint, channelMessage);
            //}
        }
    }
}