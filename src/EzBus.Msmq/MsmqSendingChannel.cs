using System;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Xml.Serialization;

namespace EzBus.Msmq
{
    public class MsmqSendingChannel : ISendingChannel
    {
        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            var queueName = MsmqAddressHelper.GetQueueName(destination);
            var queuePath = MsmqAddressHelper.GetQueuePath(destination);
            if (!MessageQueue.Exists(queueName)) throw new Exception(string.Format("Destination {0} does not exist.", destination));

            var destinationQueue = new MessageQueue(queuePath);

            var queueMessage = new Message
            {
                BodyStream = channelMessage.BodyStream,
                Label = channelMessage.Headers.First().Value,
                Extension = ConvertHeaders(channelMessage)
            };

            using (var tx = new MessageQueueTransaction())
            {
                tx.Begin();
                destinationQueue.Send(queueMessage, tx);
                tx.Commit();
            }
        }

        private static byte[] ConvertHeaders(ChannelMessage message)
        {
            var xmlSerializer = new XmlSerializer(typeof(MessageHeader[]));
            var textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, message.Headers.ToArray());
            var xml = textWriter.ToString();
            textWriter.Close();
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(xml);
        }
    }
}