using System;
using System.Reflection;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class ChannelMessageFactory
    {
        public static ChannelMessage CreateChannelMessage(object message, IMessageSerializer messageSerializer)
        {
            if (messageSerializer == null) throw new ArgumentNullException(nameof(messageSerializer));
            var messageType = message.GetType();
            var stream = messageSerializer.Serialize(message);
            var channelMessage = new ChannelMessage(stream);
            channelMessage.AddHeader(MessageHeaders.MessageType, messageType.FullName);
            channelMessage.AddHeader(MessageHeaders.UserPrincipal, Environment.UserName);
            channelMessage.AddHeader(MessageHeaders.SendingMachine, Environment.MachineName);
            channelMessage.AddHeader(MessageHeaders.SendingModule, ResolveAssemblyFullName());
            return channelMessage;
        }

        private static string ResolveAssemblyFullName()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            return assembly.ManifestModule.Assembly.FullName;
        }
    }
}