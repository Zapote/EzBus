using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Xml.Serialization;

namespace EzBus.Msmq.Channels
{
    public class MsmqReceivingChannel : IReceivingChannel
    {
        private MessageQueue inputQueue;

        public MsmqReceivingChannel()
        {
            InstanceId = Guid.NewGuid().GetHashCode().ToString(CultureInfo.InvariantCulture);
        }

        public string InstanceId { get; set; }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            inputQueue = MsmqUtilities.GetQueue(inputAddress) ?? MsmqUtilities.CreateQueue(inputAddress);

            if (!MsmqUtilities.QueueExists(errorAddress))
            {
                MsmqUtilities.CreateQueue(errorAddress);
            }

            inputQueue.MessageReadPropertyFilter = new MessagePropertyFilter
            {
                Body = true,
                Recoverable = true,
                Id = true,
                CorrelationId = true,
                Extension = true,
                AppSpecific = true,
            };

            inputQueue.ReceiveCompleted += OnReceiveCompleted;
            inputQueue.PeekCompleted += OnPeekCompleted;
            inputQueue.BeginPeek();
        }

        public Action<ChannelMessage> OnMessage { get; set; }

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var m = inputQueue.EndReceive(e.AsyncResult);

            if (OnMessage != null)
            {
                var headers = GetMessageHeaders(m);
                var message = new ChannelMessage(e.Message.BodyStream);
                message.AddHeader(headers);
                OnMessage(message);
            }

            inputQueue.BeginReceive();
        }

        private void OnPeekCompleted(object sender, PeekCompletedEventArgs e)
        {
            if (OnMessage == null) return;

            var transaction = new MessageQueueTransaction();
            transaction.Begin();

            try
            {
                var queueMessage = inputQueue.Receive(transaction);
                var headers = GetMessageHeaders(queueMessage);
                var message = new ChannelMessage(queueMessage.BodyStream);
                message.AddHeader(headers);
                OnMessage(message);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Abort();
                throw;
            }
            finally
            {
                inputQueue.BeginPeek();
            }
        }

        private static MessageHeader[] GetMessageHeaders(Message m)
        {
            var xmlHeaders = System.Text.Encoding.Default.GetString(m.Extension);
            if (string.IsNullOrEmpty(xmlHeaders)) return new MessageHeader[0];
            var serializer = new XmlSerializer(typeof(List<MessageHeader>));
            var headers = (List<MessageHeader>)serializer.Deserialize(new StringReader(xmlHeaders));
            return headers.ToArray();
        }
    }
}
