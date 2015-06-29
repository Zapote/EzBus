using System;
using System.Linq;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class ServiceBusSendingChannel : ISendingChannel
    {
        private readonly NamespaceManager namespaceManager;

        public ServiceBusSendingChannel()
        {
            var connectionString = ConnectionStringHelper.GetServiceBusConnectionString();
            namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
        }

        public void Send(EndpointAddress destination, ChannelMessage channelMessage)
        {
            var queueName = destination.QueueName;
            if (!namespaceManager.QueueExists(queueName)) throw new Exception(string.Format("Destination {0} does not exist.", destination));

            var sendingClient = QueueClient.Create(queueName);
            var message = CreateBrokeredMessage(channelMessage);
            sendingClient.Send(message);
        }

        private static BrokeredMessage CreateBrokeredMessage(ChannelMessage channelMessage)
        {
            var message = new BrokeredMessage(channelMessage.BodyStream)
            {
                Label = channelMessage.Headers.First().Value
            };

            foreach (var header in channelMessage.Headers)
            {
                message.Properties.Add(header.Name, header.Value);
            }

            return message;
        }
    }
}