using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EzBus.Core.Config;
using EzBus.Core.Serilizers;

namespace EzBus.Core
{
    public class Bus : IBus
    {
        private readonly ISendingChannel sendingChannel;
        private readonly XmlMessageSerializer serializer;
        private readonly IDictionary<string, string> messageRouting = new Dictionary<string, string>();

        public Bus(ISendingChannel sendingChannel)
        {
            if (sendingChannel == null) throw new ArgumentNullException("sendingChannel");
            this.sendingChannel = sendingChannel;
            serializer = new XmlMessageSerializer();

            var destinations = DestinationSection.Section.Destinations;

            foreach (DestinationElement destination in destinations)
            {
                messageRouting.Add(destination.Assembly, destination.Endpoint);
            }
        }

        public void Send(object message)
        {
            var messageAssembly = message.GetType().Assembly;
            var address = messageRouting[messageAssembly.GetName().Name];
            Send(address, message);
        }

        public void Send(string destinationQueue, object message)
        {
            var stream = serializer.Serialize(message);
            var channelMessage = CreateChannelMessage(message.GetType(), stream);
            sendingChannel.Send(EndpointAddress.Parse(destinationQueue), channelMessage);
        }

        private static ChannelMessage CreateChannelMessage(Type messageType, Stream stream)
        {
            var channelMessage = new ChannelMessage(stream);
            channelMessage.AddHeader("MessageType", messageType.FullName);
            channelMessage.AddHeader("UserPrincipal", Environment.UserName);
            channelMessage.AddHeader("SendingMachine", Environment.MachineName);
            channelMessage.AddHeader("SendingModule", Assembly.GetCallingAssembly().ManifestModule.Assembly.FullName);
            return channelMessage;
        }

        public void Publish(object message)
        {
            throw new NotImplementedException();
        }
    }
}