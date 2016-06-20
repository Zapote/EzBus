using System;
using EzBus.Config;
using EzBus.Logging;
using EzBus.Serializers;
using EzBus.WindowsAzure.ServiceBus.Channels;

namespace EzBus.WindowsAzure.ServiceBus.Subscription
{
    public class ServiceBusSubscriptionManager : IStartupTask
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(ServiceBusSubscriptionManager));
        private readonly ISubscriptionCollection subscriptions;
        private readonly IMessageSerializer messageSerializer;
        private readonly IBusConfig busConfig;

        public ServiceBusSubscriptionManager(ISubscriptionCollection subscriptions, IMessageSerializer messageSerializer, IBusConfig busConfig)
        {
            if (subscriptions == null) throw new ArgumentNullException(nameof(subscriptions));
            if (messageSerializer == null) throw new ArgumentNullException(nameof(messageSerializer));
            if (busConfig == null) throw new ArgumentNullException(nameof(busConfig));
            this.subscriptions = subscriptions;
            this.messageSerializer = messageSerializer;
            this.busConfig = busConfig;
        }

        public void Run()
        {
            var endpointName = busConfig.EndpointName;

            foreach (ISubscription subscription in subscriptions)
            {
                var subscriptionMessage = new SubscriptionMessage
                {
                    Endpoint = $"{endpointName}@{Environment.MachineName}"
                };

                var destination = EndpointAddress.Parse(subscription.Endpoint);

                log.Verbose($"Subscribing to: {destination}");

                var channelMessage = ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, messageSerializer);
                var sendingChannel = new ServiceBusSendingChannel();
                sendingChannel.Send(destination, channelMessage);
            }
        }
    }
}