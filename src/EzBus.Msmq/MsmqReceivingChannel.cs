using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Xml.Serialization;

namespace EzBus.Msmq
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
            var inputQueueName = MsmqAddressHelper.GetQueueName(inputAddress);
            var inputQueuePath = MsmqAddressHelper.GetQueuePath(inputAddress);
            var errorQueueName = MsmqAddressHelper.GetQueueName(errorAddress);

            inputQueue = MessageQueue.Exists(inputQueueName) ? new MessageQueue(inputQueuePath, QueueAccessMode.Receive) : MessageQueue.Create(inputQueueName, true);

            if (!MessageQueue.Exists(errorQueueName))
            {
                MessageQueue.Create(errorQueueName, true);
            }

            inputQueue.MessageReadPropertyFilter = new MessagePropertyFilter
            {
                Body = true,
                Recoverable = true,
                Id = true,
                CorrelationId = true,
                Extension = true,
                AppSpecific = true
            };

            inputQueue.ReceiveCompleted += OnReceiveCompleted;
            inputQueue.BeginReceive();
        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var m = inputQueue.EndReceive(e.AsyncResult);

            if (OnMessageReceived != null)
            {
                var headers = GetMessageHeaders(m);
                var message = new ChannelMessage(e.Message.BodyStream);
                message.AddHeader(headers);
                OnMessageReceived(this, new MessageReceivedEventArgs { Message = message });
            }

            inputQueue.BeginReceive();
        }

        private static MessageHeader[] GetMessageHeaders(Message m)
        {
            var xmlHeaders = System.Text.Encoding.Default.GetString(m.Extension);
            var serializer = new XmlSerializer(typeof(List<MessageHeader>));
            var headers = (List<MessageHeader>)serializer.Deserialize(new StringReader(xmlHeaders));
            return headers.ToArray();
        }
    }
}
