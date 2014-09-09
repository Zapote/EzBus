using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageChannel : ISendingChannel, IReceivingChannel
    {
        private static readonly List<EndpointAddress> sentDestinations = new List<EndpointAddress>();
        private static event EventHandler<MessageReceivedEventArgs> InnerMessageHandler;

        public FakeMessageChannel()
        {
            sentDestinations.Clear();
            InnerMessageHandler = null;
        }

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            sentDestinations.Add(destination);
            if (destination.QueueName.EndsWith("error")) return;

            if (InnerMessageHandler != null) InnerMessageHandler(this, new MessageReceivedEventArgs
            {
                Message = channelMessage
            });
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {

        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived
        {
            add
            {
                InnerMessageHandler += value;
            }
            remove
            {
                InnerMessageHandler -= value;
            }
        }

        public EndpointAddress LastSentDestination { get { return sentDestinations.Last(); } }

        public IEnumerable<EndpointAddress> GetSentDestinations()
        {
            return sentDestinations;
        }
    }
}