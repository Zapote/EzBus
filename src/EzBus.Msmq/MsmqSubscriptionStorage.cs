using System.Collections.Generic;
using System.Linq;
using System.Messaging;

namespace EzBus.Msmq
{
    public class MsmqSubscriptionStorage : IStartupTask
    {
        private static readonly List<MsmqSubscriptionStorageItem> subscriptions = new List<MsmqSubscriptionStorageItem>();
        private static EndpointAddress storageAddress;
        private MessageQueue storageQueue;

        private void GetQueue()
        {
            storageQueue = MsmqUtilities.GetQueue(storageAddress);
            storageQueue.Formatter = new XmlMessageFormatter(new[] { typeof(MsmqSubscriptionStorageItem) });
        }

        public void Store(string endpoint, string messageType)
        {
            if (IsSubcriber(endpoint, messageType)) return;

            CreateQueueIfNotExists();
            GetQueue();

            var item = new MsmqSubscriptionStorageItem
            {
                Endpoint = endpoint,
                MessageType = messageType
            };

            using (var tx = new MessageQueueTransaction())
            {
                var msg = new Message(item) { Label = item.Endpoint };
                tx.Begin();
                storageQueue.Send(msg, tx);
                tx.Commit();
            }

            subscriptions.Add(item);
        }

        private static bool IsSubcriber(string endpoint, string messageType)
        {
            var result = subscriptions.Where(x => x.Endpoint == endpoint).ToList();
            return result.Any(x => string.IsNullOrEmpty(x.MessageType)) || result.Any(x => x.MessageType == messageType);
        }

        private static void CreateQueueIfNotExists()
        {
            if (MsmqUtilities.QueueExists(storageAddress)) return;
            MsmqUtilities.CreateQueue(storageAddress);
        }

        public IEnumerable<string> GetSubscribersEndpoints(string messageType)
        {
            return subscriptions
                .Where(x => x.MessageType == messageType || string.IsNullOrEmpty(x.MessageType))
                .Select(x => x.Endpoint);
        }

        void IStartupTask.Run(IHostConfig config)
        {
            storageAddress = new EndpointAddress($"{config.ErrorEndpointName}.subscriptions");

            if (!MsmqUtilities.QueueExists(storageAddress)) return;

            subscriptions.Clear();

            GetQueue();

            foreach (var message in storageQueue.GetAllMessages())
            {
                var item = message.Body as MsmqSubscriptionStorageItem;
                if (item == null) continue;
                subscriptions.Add(item);
            }
        }
    }
}