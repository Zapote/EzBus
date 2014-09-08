using System;
using System.IO;
using System.Reflection;
using EzBus.Core.Routing;
using EzBus.Core.Serilizers;

namespace EzBus.Core
{
    public class Bus : IBus
    {
        private readonly ISendingChannel sendingChannel;
        private readonly IMessageRouting messageRouting;
        private readonly ISubscriptionStorage subscriptionStorage;
        private readonly XmlMessageSerializer serializer;

        public Bus(ISendingChannel sendingChannel, IMessageRouting messageRouting, ISubscriptionStorage subscriptionStorage)
        {
            if (sendingChannel == null) throw new ArgumentNullException("sendingChannel");
            if (messageRouting == null) throw new ArgumentNullException("messageRouting");
            if (subscriptionStorage == null) throw new ArgumentNullException("subscriptionStorage");
            this.sendingChannel = sendingChannel;
            this.messageRouting = messageRouting;
            this.subscriptionStorage = subscriptionStorage;
            serializer = new XmlMessageSerializer();
        }

        public void Send(object message)
        {
            var assemblyName = message.GetType().Assembly.GetName().Name;
            var address = messageRouting.GetRoute(assemblyName, message.GetType().FullName);
            Send(address, message);
        }

        public void Send(string destinationQueue, object message)
        {
            var stream = serializer.Serialize(message);
            var channelMessage = CreateChannelMessage(message.GetType(), stream);
            var destination = EndpointAddress.Parse(destinationQueue);
            channelMessage.AddHeader("Destination", destination.ToString());
            sendingChannel.Send(destination, channelMessage);
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
            var endpoints = subscriptionStorage.GetSubscribersEndpoints(message.GetType());

            foreach (var endpoint in endpoints)
            {
                Send(endpoint, message);
            }
        }
    }
}