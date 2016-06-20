using System;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQPublishingChannel : RabbitMQChannel, IPublishingChannel
    {
        private readonly IBusConfig busConfig;

        public RabbitMQPublishingChannel(IChannelFactory channelFactory, IBusConfig busConfig)
            : base(channelFactory)
        {
            if (busConfig == null) throw new ArgumentNullException(nameof(busConfig));
            this.busConfig = busConfig;
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