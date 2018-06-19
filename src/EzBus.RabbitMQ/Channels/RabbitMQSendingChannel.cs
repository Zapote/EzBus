using System;
using EzBus.Utils;

namespace EzBus.RabbitMQ.Channels
{
    [CLSCompliant(false)]
    public class RabbitMQSendingChannel : RabbitMQChannel, ISendingChannel
    {
        public RabbitMQSendingChannel(IChannelFactory cf)
            : base(cf)
        {
        }

        public void Send(EndpointAddress dest, ChannelMessage cm)
        {
            DeclareQueuePassive(dest.Name);
            var properties = ConstructHeaders(cm);
            var body = cm.BodyStream.ToByteArray();

            lock (channel)
            {
                channel.BasicPublish(string.Empty,
                    dest.Name,
                    basicProperties: properties,
                    body: body,
                    mandatory: true);
            }
        }
    }
}

