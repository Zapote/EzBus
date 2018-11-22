using System;
using EzBus.Utils;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ.Channels
{
    [CLSCompliant(false)]
    public class RabbitMQPublishingChannel : RabbitMQChannel, IPublishingChannel
    {
        private readonly IBusConfig busConfig;
        private static readonly object syncRoot = new object();

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

            lock (syncRoot)
            {
                Channel.BasicPublish(exchange, string.Empty, properties, body);
            }
        }
    }
}