using System.Messaging;
using System.Security.Principal;

namespace EzBus.Msmq
{
    public class MsmqUtilities
    {
        public static MessageQueue CreateQueue(EndpointAddress address, bool isTransactional = true)
        {
            var name = address.GetQueueName();
            var queue = MessageQueue.Create(name, isTransactional);
            SetPermissions(queue);
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

        private static void SetPermissions(MessageQueue queue)
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