using System;
using System.Globalization;
using System.Messaging;
using System.Threading;

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

        public void Initialize(EndpointAddress inputAddress)
        {
            var queueName = MsmqAddressHelper.GetQueueName(inputAddress);
            var queuePath = MsmqAddressHelper.GetQueueName(inputAddress);

            inputQueue = MessageQueue.Exists(queueName) ? new MessageQueue(queuePath, QueueAccessMode.Receive) : MessageQueue.Create(queuePath, true);
            inputQueue.ReceiveCompleted += OnReceiveCompleted;
            inputQueue.BeginReceive();
        }

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            Console.WriteLine("Got a message!{0} {1}", DateTime.Now.Millisecond, InstanceId);
            if (InstanceId == "8")
                Thread.Sleep(10);
            Thread.Sleep(3);
            inputQueue.BeginReceive();
        }
    }
}
