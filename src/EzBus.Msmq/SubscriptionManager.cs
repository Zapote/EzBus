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
        private readonly IMessageSerializer messageSerializer;

        public SubscriptionManager(IBusConfig busConfig, IMessageSerializer messageSerializer)
        {
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
            this.messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
        }

        public void Subscribe(string endpoint)
        {
            Subscribe(endpoint, string.Empty);
        }

        public void Subscribe(string endpoint, string messageName)
        {
            var subscriptionMessage = new SubscriptionMessage
            {
                Endpoint = $"{busConfig.EndpointName}@{Environment.MachineName}",
                MessageName = messageName
            };

            var destination = EndpointAddress.Parse(endpoint);

            var logMsg = messageName == string.Empty ? "All" : messageName;
            log.Info($"Subscribing to endpoint '{destination}'. Message '{logMsg}'");

            var channelMessage = ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, messageSerializer);
            var msmqSendingChannel = new MsmqSendingChannel();
            msmqSendingChannel.Send(destination, channelMessage);
        }

        public void Unsubscribe(string endpoint)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(string endpoint, string messageName)
        {
            throw new NotImplementedException();
        }
    }
}