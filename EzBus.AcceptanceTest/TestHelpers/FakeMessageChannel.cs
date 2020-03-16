using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.AcceptanceTest.TestHelpers
{
    public class FakeMessageChannel 
    {
        private static readonly List<EndpointAddress> sentDestinations = new List<EndpointAddress>();
        private static Action<BasicMessage> onMessage;

        public void Send(EndpointAddress destination, BasicMessage channelMessage)
        {
            channelMessage.BodyStream.Seek(0, 0);
            sentDestinations.Add(destination);
            if (destination.Name.EndsWith("error")) return;

            OnMessage(channelMessage);
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {

        }

        public Action<BasicMessage> OnMessage
        {
            get => onMessage; set => onMessage = value;
        }

        public static EndpointAddress LastSentDestination => sentDestinations.LastOrDefault();

        public static bool HasBeenSentToDestination(string destination)
        {
            return sentDestinations.Any(x => x.Name == destination);
        }

        public static IEnumerable<EndpointAddress> GetSentDestinations()
        {
            return sentDestinations;
        }

        public void Publish(BasicMessage channelMessage)
        {
            throw new NotImplementedException();
        }
    }
}