using System;

namespace EzBus
{
    public interface IReceivingChannel
    {
        void Initialize(EndpointAddress inputAddress);
        event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
    }
}