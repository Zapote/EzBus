using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageChannel : ISendingChannel, IReceivingChannel
    {
        private static List<EndpointAddress> sentDestinations = new List<EndpointAddress>();
        private static event EventHandler<MessageReceivedEventArgs> InnerMessageHandler;

        public static void Reset()
        {
            sentDestinations.Clear();
            sentDestinations = new List<EndpointAddress>();
        }

        public FakeMessageChannel()
        {
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

        public static EndpointAddress LastSentDestination
        {
            get
            {
                return sentDestinations.LastOrDefault();
            }
        }

        public static IEnumerable<EndpointAddress> GetSentDestinations()
        {
            return sentDestinations;
        }
    }
}