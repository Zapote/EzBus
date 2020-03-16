using System;
using EzBus.Utils;

namespace EzBus.RabbitMQ.Channels
{
    public class RabbitMQSendingChannel : RabbitMQChannel
    {
        private static readonly object syncRoot = new object();


        public RabbitMQSendingChannel(IChannelFactory channelFactory)
            : base(channelFactory)
        {
        }

        public void Send(EndpointAddress dest, BasicMessage m)
        {
            DeclareQueuePassive(dest.Name);
            var properties = ConstructHeaders(m);
            var body = m.BodyStream.ToByteArray();

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


