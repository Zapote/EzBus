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
                DefaultMessageTimeToLive = new TimeSpan(0, 1, 0)
            };

            namespaceManager.CreateQueue(qd);
        }
    }
}
