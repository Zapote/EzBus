using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core
{
    //TODO: Move to separate project and use NamnedPipes
    public class InMemoryMessageChannel : ISendingChannel, IReceivingChannel, IPublishingChannel
    {
        private static List<EndpointAddress> sentDestinations = new List<EndpointAddress>();

        public static void Reset()
        {
            sentDestinations.Clear();
            sentDestinations = new List<EndpointAddress>();
        }

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            sentDestinations.Add(destination);
            if (destination.QueueName.EndsWith("error")) return;
            OnMessage?.Invoke(channelMessage);
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {

        }

        public Action<ChannelMessage> OnMessage { get; set; }

        public static EndpointAddress LastSentDestination => sentDestinations.LastOrDefault();

        public static IEnumerable<EndpointAddress> GetSentDestinations()
        {
            return sentDestinations;
        }

        public void Publish(ChannelMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            OnMessage?.Invoke(channelMessage);
        }
    }
}