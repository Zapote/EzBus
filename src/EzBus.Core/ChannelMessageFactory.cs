using System;
using System.Reflection;
using EzBus.Serilizers;

namespace EzBus.Core
{
    public class ChannelMessageFactory
    {
        public static ChannelMessage CreateChannelMessage(object message, IMessageSerilizer messageSerilizer)
        {
            if (messageSerilizer == null) throw new ArgumentNullException("messageSerilizer");
            var messageType = message.GetType();
            var stream = messageSerilizer.Serialize(message);
            var channelMessage = new ChannelMessage(stream);
            channelMessage.AddHeader("MessageType", messageType.FullName);
            channelMessage.AddHeader("UserPrincipal", Environment.UserName);
            channelMessage.AddHeader("SendingMachine", Environment.MachineName);
            channelMessage.AddHeader("SendingModule", Assembly.GetCallingAssembly().ManifestModule.Assembly.FullName);
            return channelMessage;
        }
    }
}