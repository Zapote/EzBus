using System;
using System.IO;
using System.Reflection;
using EzBus.Core.Serilizers;

namespace EzBus.Core
{
    public class Bus : IBus
    {
        private readonly ISendingChannel sendingChannel;
        private readonly XmlMessageSerializer serializer;

        public Bus(ISendingChannel sendingChannel)
        {
            if (sendingChannel == null) throw new ArgumentNullException("sendingChannel");
            this.sendingChannel = sendingChannel;
            serializer = new XmlMessageSerializer();
        }

        public void Send(object message)
        {
            //GET Address from conf
            //sendingChannel.Send(new EndpointAddress("qq"), message);
        }

        public void Send(string destinationQueue, object message)
        {
            var stream = serializer.Serialize(message);
            var channelMessage = CreateChannelMessage(message.GetType(), stream);
            sendingChannel.Send(new EndpointAddress(destinationQueue), channelMessage);
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