using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using EzBus.Logging;
using EzBus.Serializers;

namespace EzBus.Msmq.Subscription
{
    public class MsmqSubscriptionStorage : ISubscriptionStorage
    {
        private static readonly ILogger log = LogManager.GetLogger(typeof(MsmqSubscriptionStorage));
        private readonly IBodySerializer bodySerializer;
        private readonly IBusConfig busConfig;
        private readonly List<MsmqSubscriptionStorageItem> subscriptions = new List<MsmqSubscriptionStorageItem>();
        private EndpointAddress storageAddress;
        private MessageQueue storageQueue;

        public MsmqSubscriptionStorage(IBusConfig busConfig, IBodySerializer bodySerializer)
        {
            this.bodySerializer = bodySerializer ?? throw new ArgumentNullException(nameof(bodySerializer));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
        }

        private void GetQueue()
        {
            storageQueue = MsmqUtilities.GetQueue(storageAddress);
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
            using (var stream = new MemoryStream())
            {
                bodySerializer.Serialize(item, stream);

                var msg = new Message
                {
                    BodyStream = stream,
                    Label = item.Endpoint
                };

                tx.Begin();
                storageQueue.Send(msg, tx);
                tx.Commit();
            }

            subscriptions.Add(item);
        }

        private bool IsSubcriber(string endpoint, string messageType)
        {
            var result = subscriptions.Where(x => x.Endpoint == endpoint).ToList();
            return result.Any(x => string.IsNullOrEmpty(x.MessageType)) || result.Any(x => x.MessageType == messageType);
        }

        private void CreateQueueIfNotExists()
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

        public void Initialize()
        {
            storageAddress = new EndpointAddress($"{busConfig.EndpointName}.subscriptions");

            if (!MsmqUtilities.QueueExists(storageAddress)) return;

            subscriptions.Clear();

            GetQueue();

            foreach (var message in storageQueue.GetAllMessages())
            {
                var item = bodySerializer.Deserialize(message.BodyStream, typeof(MsmqSubscriptionStorageItem)) as MsmqSubscriptionStorageItem;
                if (item == null) continue;
                subscriptions.Add(item);
                log.Info($"{item.Endpoint} registered as subscriber.");
            }
        }
    }
}