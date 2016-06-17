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
        private readonly IHostConfig hostConfig;

        public ServiceBusSubscriptionManager(ISubscriptionCollection subscriptions, IMessageSerializer messageSerializer, IHostConfig hostConfig)
        {
            if (subscriptions == null) throw new ArgumentNullException(nameof(subscriptions));
            if (messageSerializer == null) throw new ArgumentNullException(nameof(messageSerializer));
            if (hostConfig == null) throw new ArgumentNullException(nameof(hostConfig));
            this.subscriptions = subscriptions;
            this.messageSerializer = messageSerializer;
            this.hostConfig = hostConfig;
        }

        public void Run()
        {
            var endpointName = hostConfig.EndpointName;

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