using System;
using EzBus.Utils;

namespace EzBus.RabbitMQ.Channels
{
    [CLSCompliant(false)]
    public class RabbitMQSendingChannel : RabbitMQChannel, ISendingChannel
    {
        private static readonly object syncRoot = new object();


        public RabbitMQSendingChannel(IChannelFactory channelFactory)
            : base(channelFactory)
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
                    destination.Name,
                    basicProperties: properties,
                    body: body,
                    mandatory: true);
            }
        }
    }
}


