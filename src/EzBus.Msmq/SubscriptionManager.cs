using System;
using EzBus.Logging;
using EzBus.Msmq.Channels;
using EzBus.Serializers;

namespace EzBus.Msmq
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(SubscriptionManager));
        private readonly IBusConfig busConfig;
        private readonly IBodySerializer bodySerializer;

        public SubscriptionManager(IBusConfig busConfig, IBodySerializer bodySerializer)
        {
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
            this.bodySerializer = bodySerializer ?? throw new ArgumentNullException(nameof(bodySerializer));
        }

        public void Subscribe(string endpoint)
        {
            var subscriptionMessage = new SubscriptionMessage
            {
                Endpoint = $"{busConfig.EndpointName}@{Environment.MachineName}"
            };

            var destination = EndpointAddress.Parse(endpoint);

            log.Info($"Subscribing to endpoint '{destination}'");

            var channelMessage = ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, bodySerializer);
            var msmqSendingChannel = new MsmqSendingChannel();
            msmqSendingChannel.Send(destination, channelMessage);
        }
    }
}