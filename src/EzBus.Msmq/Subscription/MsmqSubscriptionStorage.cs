using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using EzBus.Logging;
using EzBus.Serializers;
using EzBus.Utils;

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

        public void Store(string endpoint, string messageName)
        {
            if (IsSubcriber(endpoint, messageName)) return;

            CreateQueueIfNotExists();

            var item = new MsmqSubscriptionStorageItem
            {
                Endpoint = endpoint,
                MessageName = messageName
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

        public void Remove(string endpoint, string messageName)
        {
            var toBeStored = new List<MsmqSubscriptionStorageItem>();

            foreach (var message in storageQueue.GetAllMessages())
            {
                var item = bodySerializer.Deserialize(message.BodyStream, typeof(MsmqSubscriptionStorageItem)) as MsmqSubscriptionStorageItem;
                if (item == null) continue;

                if (item.Endpoint == endpoint && item.MessageName == messageName) continue;
                toBeStored.Add(item);
            }

            using (var tx = new MessageQueueTransaction())
            using (var stream = new MemoryStream())
            {
                tx.Begin();

                storageQueue.Purge();

                foreach (var item in toBeStored)
                {
                    bodySerializer.Serialize(item, stream);

                    var msg = new Message
                    {
                        BodyStream = stream,
                        Label = item.Endpoint
                    };

                    storageQueue.Send(msg, tx);
                }

                tx.Commit();
            }

            var remove = subscriptions.FirstOrDefault(x => x.Endpoint == endpoint && x.MessageName == messageName);
            if (remove != null) subscriptions.Remove(remove);
        }

        private bool IsSubcriber(string endpoint, string messageType)
        {
            var result = subscriptions.Where(x => x.Endpoint == endpoint).ToList();
            return result.Any(x => x.MessageName.IsNullOrEmpty()) || result.Any(x => x.MessageName == messageType);
        }

        private void CreateQueueIfNotExists()
        {
            if (MsmqUtilities.QueueExists(storageAddress)) return;
            MsmqUtilities.CreateQueue(storageAddress);
        }

        public IEnumerable<string> GetSubscribers(string messageName)
        {
            return subscriptions
                .Where(x => x.MessageName == messageName || string.IsNullOrEmpty(x.MessageName))
                .Select(x => x.Endpoint);
        }

        public void Initialize()
        {
            storageAddress = new EndpointAddress($"{busConfig.EndpointName}.subscriptions");

            if (!MsmqUtilities.QueueExists(storageAddress)) return;

            subscriptions.Clear();

            storageQueue = MsmqUtilities.GetQueue(storageAddress);

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