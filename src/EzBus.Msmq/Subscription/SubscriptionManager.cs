using System;
using EzBus.Config;
using EzBus.Logging;
using EzBus.Msmq.Channels;
using EzBus.Serializers;

namespace EzBus.Msmq.Subscription
{
    public class MsmqSubscriptionManager : IStartupTask
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(MsmqSubscriptionManager));
        private readonly IEzBusConfig config;
        private readonly IMessageSerializer messageSerializer;
        private readonly IBusConfig busConfig;

        public MsmqSubscriptionManager(IEzBusConfig config, IMessageSerializer messageSerializer, IBusConfig busConfig)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Run()
        {
            var endpointName = busConfig.EndpointName;

            foreach (var subscription in config.Subscriptions)
            {
                var subscriptionMessage = new SubscriptionMessage
                {
                    Endpoint = $"{endpointName}@{Environment.MachineName}"
                };

                var destination = EndpointAddress.Parse(subscription.Endpoint);

                log.Verbose($"Subscribing to: {destination}");

                var channelMessage = ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, messageSerializer);
                var msmqSendingChannel = new MsmqSendingChannel();
                msmqSendingChannel.Send(destination, channelMessage);
            }
        }
    }
}