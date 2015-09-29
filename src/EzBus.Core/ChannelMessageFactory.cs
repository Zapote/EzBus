using System;
using System.Reflection;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class ChannelMessageFactory
    {
        public static ChannelMessage CreateChannelMessage(object message, IMessageSerializer messageSerializer)
        {
            if (messageSerializer == null) throw new ArgumentNullException("messageSerializer");
            var messageType = message.GetType();
            var stream = messageSerializer.Serialize(message);
            var channelMessage = new ChannelMessage(stream);
            channelMessage.AddHeader("MessageType", messageType.FullName);
            channelMessage.AddHeader("UserPrincipal", Environment.UserName);
            channelMessage.AddHeader("SendingMachine", Environment.MachineName);
            channelMessage.AddHeader("SendingModule", ResolveAssemblyFullName());
            return channelMessage;
        }

        private static string ResolveAssemblyFullName()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            return assembly.ManifestModule.Assembly.FullName;
        }
    }
}