using System;
using EzBus.Utils;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQPublishingChannel : RabbitMQChannel, IPublishingChannel
    {
        private readonly IBusConfig busConfig;

        public RabbitMQPublishingChannel(IChannelFactory channelFactory, IBusConfig busConfig)
            : base(channelFactory)
        {
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Publish(ChannelMessage channelMessage)
        {
            var exchange = busConfig.EndpointName.ToLower();
            var properties = ConstructHeaders(channelMessage);
            var body = channelMessage.BodyStream.ToByteArray();
            channel.BasicPublish(exchange, string.Empty, properties, body);
        }
    }
}