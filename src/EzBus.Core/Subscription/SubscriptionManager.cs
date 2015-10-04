using System;
using EzBus.Core.Resolvers;
using EzBus.Core.Serializers;
using EzBus.Logging;

namespace EzBus.Core.Subscription
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(SubscriptionManager));
        public void Subscribe(string subscribingEndpointName)
        {
            var subscriptions = Config.SubscriptionSection.Section.Subscriptions;

            foreach (Config.SubscriptionElement subscription in subscriptions)
            {
                var subscriptionMessage = new SubscriptionMessage
                {
                    Endpoint = $"{subscribingEndpointName}@{Environment.MachineName}"
                };

                var destination = EndpointAddress.Parse(subscription.Endpoint);

                log.VerboseFormat("Subscribing to: {0}", destination);

                var serializer = new XmlMessageSerializer();
                var sendingChannel = SendingChannelResolver.GetChannel();
                sendingChannel.Send(destination, ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, serializer));
            }
        }
    }
}