using System;
using EzBus.Config;
using EzBus.Core.Resolvers;
using EzBus.Core.Serializers;
using EzBus.Logging;

namespace EzBus.Core.Subscription
{
    public class DefaultSubscriptionManager : ISubscriptionManager
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(DefaultSubscriptionManager));
        private ISubscriptionCollection mySubscriptions;

        public void Subscribe(string subscribingEndpointName)
        {
            foreach (ISubscription subscription in mySubscriptions)
            {
                var subscriptionMessage = new SubscriptionMessage
                {
                    Endpoint = $"{subscribingEndpointName}@{Environment.MachineName}"
                };

                var destination = EndpointAddress.Parse(subscription.Endpoint);

                log.VerboseFormat("Subscribing to: {0}", destination);

                var serializer = MessageSerlializerResolver.GetSerializer();
                var sendingChannel = SendingChannelResolver.GetChannel();
                sendingChannel.Send(destination, ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, serializer));
            }
        }

        public void Initialize(ISubscriptionCollection subscriptions)
        {
            mySubscriptions = subscriptions;
        }
    }
}