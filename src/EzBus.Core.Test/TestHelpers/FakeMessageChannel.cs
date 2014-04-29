using System;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageChannel : ISendingChannel, IReceivingChannel
    {
        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            LastSentDestination = destination;

            if (destination.QueueName.EndsWith("error")) return;
 
            if (OnMessageReceived != null) OnMessageReceived(this, new MessageReceivedEventArgs
            {
                Message = channelMessage
            });
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {

        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public EndpointAddress LastSentDestination { get; private set; }
    }
}