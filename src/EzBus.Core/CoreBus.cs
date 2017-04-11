using System;
using EzBus.Core.Resolvers;
using EzBus.Core.Routing;
using EzBus.Serializers;
using EzBus.Utils;

namespace EzBus.Core
{
    public class CoreBus : IBus
    {
        private readonly ISendingChannel sendingChannel;
        private readonly IPublishingChannel publishingChannel;
        private readonly IMessageRouting messageRouting;
        private readonly IMessageSerializer serializer;

        public CoreBus(ISendingChannel sendingChannel, IPublishingChannel publishingChannel, IMessageRouting messageRouting)
        {
            if (sendingChannel == null) throw new ArgumentNullException(nameof(sendingChannel));
            if (messageRouting == null) throw new ArgumentNullException(nameof(messageRouting));
            if (publishingChannel == null) throw new ArgumentNullException(nameof(publishingChannel));

            this.sendingChannel = sendingChannel;
            this.messageRouting = messageRouting;
            this.publishingChannel = publishingChannel;

            var messageSerializerType = TypeResolver.GetType<IMessageSerializer>();
            serializer = (IMessageSerializer)messageSerializerType.CreateInstance();
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
            channelMessage.AddHeader(MessageHeaders.Destination, destination.ToString());
            sendingChannel.Send(destination, channelMessage);
        }

        public void Publish(object message)
        {
            var channelMessage = ChannelMessageFactory.CreateChannelMessage(message, serializer);
            publishingChannel.Publish(channelMessage);
        }
    }
}