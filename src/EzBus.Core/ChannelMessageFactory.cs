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