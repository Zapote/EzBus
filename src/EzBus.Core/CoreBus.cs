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
            this.sendingChannel = sendingChannel ?? throw new ArgumentNullException(nameof(sendingChannel));
            this.messageRouting = messageRouting ?? throw new ArgumentNullException(nameof(messageRouting));
            this.publishingChannel = publishingChannel ?? throw new ArgumentNullException(nameof(publishingChannel));

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