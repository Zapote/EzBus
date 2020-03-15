using System;

namespace EzBus
{
    public interface IReceivingChannel
    {
        void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress);
        Action<ChannelMessage> OnMessage { get; set; }
    }
}