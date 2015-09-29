using System;

namespace EzBus.RabbitMQ
{
    public class RabbitMQSendingChannel : ISendingChannel
    {
        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            throw new NotImplementedException();
        }
    }
}
