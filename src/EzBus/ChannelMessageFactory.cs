using System;
using System.IO;
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
            var stream = new MemoryStream();
            messageSerializer.Serialize(message, stream);
            var channelMessage = new ChannelMessage(stream);
            channelMessage.AddHeader(MessageHeaders.MessageFullname, messageType.FullName);
            channelMessage.AddHeader(MessageHeaders.MessageName, messageType.Name);
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