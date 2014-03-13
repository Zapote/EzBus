using System;
using System.Messaging;
using System.Text;

namespace EzBus.Msmq
{
    public class MsmqMessageSender : ISendingChannel
    {
        public void Send(EndpointAddress destination, MessageEnvelope message)
        {
            var queueName = MsmqAddressHelper.GetQueueName(destination);
            var queuePath = MsmqAddressHelper.GetQueueName(destination);
            if (!MessageQueue.Exists(queueName)) throw new Exception(string.Format("Destination {0} does not exist.", destination));

            var destinationQueue = new MessageQueue(queuePath);

            var tx = new MessageQueueTransaction();

            tx.Begin();

            var queueMessage = new Message
            {
                BodyStream = message.BodyStream,
                Label = message.MessageType.Name,
                Extension = ConvertHeaders(message)
            };

            destinationQueue.Send(queueMessage, tx);

            tx.Commit();
        }

        private static byte[] ConvertHeaders(MessageEnvelope envelope)
        {
            var stringBuilder = new StringBuilder();

            foreach (var header in envelope.Headers)
            {
                stringBuilder.AppendLine(header.ToString());
            }

            var encoding = new UTF8Encoding();
            return encoding.GetBytes(stringBuilder.ToString());
        }
    }
}