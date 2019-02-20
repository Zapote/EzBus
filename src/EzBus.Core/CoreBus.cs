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
        private readonly IBodySerializer serializer;

        public CoreBus(ISendingChannel sendingChannel, IPublishingChannel publishingChannel)
        {
            this.sendingChannel = sendingChannel ?? throw new ArgumentNullException(nameof(sendingChannel));
            this.publishingChannel = publishingChannel ?? throw new ArgumentNullException(nameof(publishingChannel));

            var messageSerializerType = TypeResolver.GetType<IBodySerializer>();
            serializer = (IBodySerializer)messageSerializerType.CreateInstance();
        }

        public void Send(string endpoint, object message)
        {
            var channelMessage = ChannelMessageFactory.CreateChannelMessage(message, serializer);
            var destination = EndpointAddress.Parse(endpoint);
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