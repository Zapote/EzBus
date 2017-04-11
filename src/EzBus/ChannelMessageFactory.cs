using System;
using System.Reflection;
using EzBus.Serializers;

namespace EzBus
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
            channelMessage.AddHeader(MessageHeaders.UserPrincipal, "TODO"); //TODO: get current user
            channelMessage.AddHeader(MessageHeaders.SendingMachine, Environment.MachineName);
            channelMessage.AddHeader(MessageHeaders.SendingModule, ResolveAssemblyName());
            channelMessage.AddHeader(MessageHeaders.TimeSent, DateTime.UtcNow.ToString("O"));
            return channelMessage;
        }

        private static string ResolveAssemblyName()
        {
            var assembly = Assembly.GetEntryAssembly();
            var moduleName = assembly.ManifestModule.Assembly.GetName().Name;
            return moduleName;
        }
    }
}