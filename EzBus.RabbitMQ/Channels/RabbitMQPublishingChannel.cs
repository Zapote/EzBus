using System;
using EzBus.Utils;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQPublishingChannel : RabbitMQChannel
    {
        private readonly EzBus.IBusConfig busConfig;

        public RabbitMQPublishingChannel(IChannelFactory cf, EzBus.IBusConfig busConfig)
            : base(cf)
        {
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        public void Publish(BasicMessage cm)
        {
            var exchange = busConfig.Address.ToLower();
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