using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;

namespace EzBus.Msmq.Subscription
{
    public class MsmqSubscriptionStorage : ISubscriptionStorage
    {
        private readonly IHostConfig hostConfig;
        private static readonly List<MsmqSubscriptionStorageItem> subscriptions = new List<MsmqSubscriptionStorageItem>();
        private static EndpointAddress storageAddress;
        private MessageQueue storageQueue;

        public MsmqSubscriptionStorage(IHostConfig hostConfig)
        {
            if (hostConfig == null) throw new ArgumentNullException(nameof(hostConfig));
            this.hostConfig = hostConfig;

            Initialize();
        }

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

        public IEnumerable<string> GetSubscribers(string messageType)
        {
            return subscriptions
                .Where(x => x.MessageType == messageType || string.IsNullOrEmpty(x.MessageType))
                .Select(x => x.Endpoint);
        }

        private void Initialize()
        {
            storageAddress = new EndpointAddress($"{hostConfig.EndpointName}.subscriptions");

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