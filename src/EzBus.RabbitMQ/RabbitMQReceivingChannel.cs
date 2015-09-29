using System;

namespace EzBus.RabbitMQ
{
    public class RabbitMQReceivingChannel : IReceivingChannel
    {
        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
    }
}