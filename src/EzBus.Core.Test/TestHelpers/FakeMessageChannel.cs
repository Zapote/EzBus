using System;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageChannel : ISendingChannel, IReceivingChannel
    {
        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            LastSentDestination = destination;
            if (OnMessageReceived != null) OnMessageReceived(this, new MessageReceivedEventArgs() { });
        }

        public void Initialize(EndpointAddress inputAddress)
        {
        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public EndpointAddress LastSentDestination { get; private set; }
    }
}