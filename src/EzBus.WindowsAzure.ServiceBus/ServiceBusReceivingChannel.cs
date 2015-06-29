using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace EzBus.WindowsAzure.ServiceBus
{
    public class ServiceBusReceivingChannel : IReceivingChannel
    {
        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            var queueName = inputAddress.QueueName;
            var queueClient = QueueClient.Create(queueName);
            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1),
                MaxConcurrentCalls = 1
            };

            var connectionString = ConnectionStringHelper.GetServiceBusConnectionString();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(queueName))
            {
                var qd = new QueueDescription(queueName)
                {
                    MaxSizeInMegabytes = 5120,
                    DefaultMessageTimeToLive = new TimeSpan(0, 1, 0)
                };

                namespaceManager.CreateQueue(qd);
            }

            queueClient.OnMessage(OnMessage, options);
        }

        private void OnMessage(BrokeredMessage message)
        {
            try
            {
                if (OnMessageReceived != null)
                {
                    var headers = ResolveMessageHeaders(message);
                    var bodyStream = message.GetBody<Stream>();
                    var channelMessage = new ChannelMessage(bodyStream);
                    channelMessage.AddHeader(headers);
                    OnMessageReceived(this, new MessageReceivedEventArgs { Message = channelMessage });
                }

                message.Complete();
            }
            catch (Exception ex)
            {
                message.Abandon();
                throw;
            }
        }

        private static MessageHeader[] ResolveMessageHeaders(BrokeredMessage m)
        {
            var headers = new List<MessageHeader>();
            foreach (var prop in m.Properties)
            {
                var value = prop.Value == null ? string.Empty : prop.Value.ToString();
                headers.Add(new MessageHeader { Name = prop.Key, Value = value });
            }
            return headers.ToArray();
        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
    }
}
