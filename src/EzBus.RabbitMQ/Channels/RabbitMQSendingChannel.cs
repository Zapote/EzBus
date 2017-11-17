using System;
using EzBus.Utils;

namespace EzBus.RabbitMQ.Channels
{
    [CLSCompliant(false)]
    public class RabbitMQSendingChannel : RabbitMQChannel, ISendingChannel
    {
        public RabbitMQSendingChannel(IChannelFactory channelFactory)
            : base(channelFactory)
        {
        }

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            DeclareQueuePassive(destination.Name);
            var properties = ConstructHeaders(channelMessage);
            var body = channelMessage.BodyStream.ToByteArray();

            lock (channel)
            {
                channel.BasicPublish(string.Empty,
                    destination.Name,
                    basicProperties: properties,
                    body: body,
                    mandatory: true);
            }
        }
    }
}

