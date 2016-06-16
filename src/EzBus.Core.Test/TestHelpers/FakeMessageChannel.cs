using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core.Test.TestHelpers
{
    public class FakeMessageChannel : ISendingChannel, IReceivingChannel, IPublishingChannel
    {
        private static List<EndpointAddress> sentDestinations = new List<EndpointAddress>();
        private static Action<ChannelMessage> onMessage;

        public FakeMessageChannel()
        {
            sentDestinations.Clear();
            sentDestinations = new List<EndpointAddress>();
        }

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            sentDestinations.Add(destination);
            if (destination.QueueName.EndsWith("error")) return;

            OnMessage(channelMessage);
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {

        }

        public Action<ChannelMessage> OnMessage { get { return onMessage; } set { onMessage = value; } }

        public static EndpointAddress LastSentDestination => sentDestinations.LastOrDefault();

        public static IEnumerable<EndpointAddress> GetSentDestinations()
        {
            return sentDestinations;
        }

        public void Publish(ChannelMessage channelMessage)
        {
            throw new NotImplementedException();
        }
    }
}