using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class QueueUtilities
    {
        public static void CreateQueue(string queueName)
        {
            var connectionString = ConnectionStringHelper.GetServiceBusConnectionString();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (namespaceManager.QueueExists(queueName)) return;

            var qd = new QueueDescription(queueName)
            {
                MaxSizeInMegabytes = 5120,
                DefaultMessageTimeToLive = TimeSpan.FromDays(14),
                LockDuration = TimeSpan.FromSeconds(30),
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(10),
                MaxDeliveryCount = 6,
                RequiresSession = false,
                SupportOrdering = true
            };

            namespaceManager.CreateQueue(qd);
        }
    }
}
