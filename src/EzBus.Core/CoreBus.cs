using System;
using EzBus.Core.Routing;
using EzBus.Core.Serilizers;
using EzBus.Core.Utils;

namespace EzBus.Core
{
    public class CoreBus : IBus
    {
        private readonly ISendingChannel sendingChannel;
        private readonly IMessageRouting messageRouting;
        private readonly ISubscriptionStorage subscriptionStorage;
        private readonly XmlMessageSerializer serializer;

        public CoreBus(ISendingChannel sendingChannel, IMessageRouting messageRouting, ISubscriptionStorage subscriptionStorage)
        {
            if (sendingChannel == null) throw new ArgumentNullException(nameof(sendingChannel));
            if (messageRouting == null) throw new ArgumentNullException(nameof(messageRouting));
            if (subscriptionStorage == null) throw new ArgumentNullException(nameof(subscriptionStorage));

            this.sendingChannel = sendingChannel;
            this.messageRouting = messageRouting;
            this.subscriptionStorage = subscriptionStorage;

            serializer = new XmlMessageSerializer();
        }

        public void Send(object message)
        {
            var assemblyName = message.GetAssemblyName();
            var address = messageRouting.GetRoute(assemblyName, message.GetType().FullName);
            Send(address, message);
        }

        public void Send(string destinationQueue, object message)
        {
            var channelMessage = ChannelMessageFactory.CreateChannelMessage(message, serializer);
            var destination = EndpointAddress.Parse(destinationQueue);
            channelMessage.AddHeader("Destination", destination.ToString());
            sendingChannel.Send(destination, channelMessage);
        }

        public void Publish(object message)
        {
            var endpoints = subscriptionStorage.GetSubscribersEndpoints(message.GetType());

            foreach (var endpoint in endpoints)
            {
                Send(endpoint, message);
            }
        }
    }
}