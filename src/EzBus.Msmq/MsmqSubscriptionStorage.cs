﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;

namespace EzBus.Msmq
{
    public class MsmqSubscriptionStorage : ISubscriptionStorage
    {
        private readonly List<MsmqSubscriptionStorageItem> subscriptions = new List<MsmqSubscriptionStorageItem>();
        private EndpointAddress storageAddress;
        private MessageQueue storageQueue;

        public void Initialize(string endpointName)
        {
            storageAddress = new EndpointAddress(endpointName + ".subscriptions");

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

        private void GetQueue()
        {
            storageQueue = MsmqUtilities.GetQueue(storageAddress);
            storageQueue.Formatter = new XmlMessageFormatter(new[] { typeof(MsmqSubscriptionStorageItem) });
        }

        public void Store(string endpoint, Type messageType)
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

        private bool IsSubcriber(string endpoint, Type messageType)
        {
            var result = subscriptions.Where(x => x.Endpoint == endpoint).ToList();
            return result.Any(x => x.MessageType == null) || result.Any(x => x.MessageType == messageType);
        }

        private void CreateQueueIfNotExists()
        {
            if (MsmqUtilities.QueueExists(storageAddress)) return;
            MsmqUtilities.CreateQueue(storageAddress);
        }

        public IEnumerable<string> GetSubscribersEndpoints(Type messageType)
        {
            return subscriptions
                .Where(x => x.MessageType == messageType || x.MessageType == null)
                .Select(x => x.Endpoint);
        }
    }
}