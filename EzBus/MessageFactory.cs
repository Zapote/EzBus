using System;
using System.IO;
using EzBus.Serializers;

namespace EzBus
{
    public class MessageFactory
    {
        public static BasicMessage Create(object message, IBodySerializer bodySerializer)
        {
            if (bodySerializer == null) throw new ArgumentNullException(nameof(bodySerializer));
            var messageType = message.GetType();
            var stream = new MemoryStream();
            bodySerializer.Serialize(message, stream);

            var channelMessage = new BasicMessage(stream);
            channelMessage.AddHeader(MessageHeaders.MessageFullname, messageType.FullName);
            channelMessage.AddHeader(MessageHeaders.MessageName, messageType.Name);
            channelMessage.AddHeader(MessageHeaders.SendingMachine, Environment.MachineName);
            channelMessage.AddHeader(MessageHeaders.TimeSent, DateTime.UtcNow.ToString("O"));

            return channelMessage;
        }
    }
}