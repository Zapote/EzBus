using System;
using System.Text;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQPublishingChannel : RabbitMQChannel, IPublishingChannel
    {
        private readonly IHostConfig hostConfig;

        public RabbitMQPublishingChannel(IChannelFactory channelFactory, IHostConfig hostConfig)
            : base(channelFactory)
        {
            if (hostConfig == null) throw new ArgumentNullException(nameof(hostConfig));
            this.hostConfig = hostConfig;
        }

        public void Publish(ChannelMessage channelMessage)
        {
            var exchange = hostConfig.EndpointName;
            var properties = ConstructHeaders(channelMessage);
            var body = channelMessage.BodyStream.ToByteArray();
            channel.BasicPublish(exchange, string.Empty, properties, body);
        }
    }
}