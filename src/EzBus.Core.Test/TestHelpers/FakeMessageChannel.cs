using System;
using System.Collections.Generic;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageChannel : ISendingChannel, IReceivingChannel
    {
        private readonly List<EndpointAddress> sentDestinations = new List<EndpointAddress>();

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            LastSentDestination = destination;
            sentDestinations.Add(destination);
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
        public IEnumerable<EndpointAddress> GetSentDestinations()
        {
            return sentDestinations;
        }
    }
}