using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ServiceBus.Messaging;

namespace EzBus.WindowsAzure.ServiceBus.Channels
{
    public class ServiceBusReceivingChannel : IReceivingChannel
    {
        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            QueueUtilities.CreateQueue(inputAddress.QueueName);

            var inputQueueClient = QueueClient.Create(inputAddress.QueueName);
            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1),
                MaxConcurrentCalls = 1
            };
            inputQueueClient.OnMessage(OnBrokeredMessage, options);
        }

        public Action<ChannelMessage> OnMessage { get; set; }

        private void OnBrokeredMessage(BrokeredMessage message)
        {
            try
            {
                if (OnMessage != null)
                {
                    var headers = ResolveMessageHeaders(message);
                    var bodyStream = message.GetBody<Stream>();
                    var channelMessage = new ChannelMessage(bodyStream);
                    channelMessage.AddHeader(headers);
                    OnMessage(channelMessage);
                }

                message.Complete();
            }
            catch (Exception)
            {
                message.DeadLetter();
                throw;
            }
        }

        private static MessageHeader[] ResolveMessageHeaders(BrokeredMessage m)
        {
            var headers = new List<MessageHeader>();
            foreach (var prop in m.Properties)
            {
                var value = prop.Value?.ToString() ?? string.Empty;
                headers.Add(new MessageHeader(prop.Key, value));
            }
            return headers.ToArray();
        }
    }
}
