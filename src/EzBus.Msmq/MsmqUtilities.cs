using System;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Security.Principal;
using System.Text;
using System.Xml.Serialization;
using EzBus.Logging;

namespace EzBus.Msmq
{
    public class MsmqUtilities
    {
        private static readonly ILogger log = LogManager.GetLogger("Ezbus.Msmq");

        public static MessageQueue CreateQueue(EndpointAddress address, bool isTransactional = true)
        {
            var name = address.GetQueueName();

            log.Info($"Creating queue {name}");

            var queue = MessageQueue.Create(name, isTransactional);
            SetQueuePermissions(queue);
            return queue;
        }

        public static bool QueueExists(EndpointAddress address)
        {
            var name = address.GetQueueName();
            return MessageQueue.Exists(name);
        }

        public static MessageQueue GetQueue(EndpointAddress address)
        {
            var path = address.GetQueuePath();
            var name = address.GetQueueName();
            return MessageQueue.Exists(name) ? new MessageQueue(path, QueueAccessMode.SendAndReceive) : null;
        }

        public static void WriteMessage(EndpointAddress destination, ChannelMessage channelMessage)
        {
            var queueName = destination.GetQueueName();
            var queuePath = destination.GetQueuePath();

            if (!MessageQueue.Exists(queueName)) throw new InvalidOperationException($"Destination {destination} does not exist.");

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

        private static void SetQueuePermissions(MessageQueue queue)
        {
            var admins = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null).Translate(typeof(NTAccount)).ToString();
            var everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null).Translate(typeof(NTAccount)).ToString();
            var anonymous = new SecurityIdentifier(WellKnownSidType.AnonymousSid, null).Translate(typeof(NTAccount)).ToString();

            queue.SetPermissions(admins, MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
            queue.SetPermissions(everyone, MessageQueueAccessRights.GenericWrite, AccessControlEntryType.Allow);
            queue.SetPermissions(anonymous, MessageQueueAccessRights.GenericWrite, AccessControlEntryType.Allow);
        }
    }
}