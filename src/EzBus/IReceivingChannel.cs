using System;

namespace EzBus
{
    public interface IReceivingChannel
    {
        void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress);
        event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
    }
}