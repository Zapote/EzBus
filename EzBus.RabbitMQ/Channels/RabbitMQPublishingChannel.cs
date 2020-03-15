using System;
using EzBus.Utils;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQPublishingChannel : RabbitMQChannel, IPublishingChannel
    {
        private readonly IBusConfig busConfig;

        public RabbitMQPublishingChannel(IChannelFactory cf, IBusConfig busConfig)
            : base(cf)
        {
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Publish(ChannelMessage cm)
        {
            var exchange = busConfig.EndpointName.ToLower();
            var properties = ConstructHeaders(cm);
            var body = cm.BodyStream.ToByteArray();
            var messageName = cm.GetHeader(MessageHeaders.MessageName);

            lock (channel)
            {
                channel.BasicPublish(exchange, string.Empty, properties, body);
            }
        }
    }
}