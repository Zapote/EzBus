using System;
using System.IO;
using System.Text;
using EzBus.Logging;
using RabbitMQ.Client.Events;

namespace EzBus.RabbitMQ.Channels
{
    [CLSCompliant(false)]
    public class RabbitMQReceivingChannel : RabbitMQChannel, IReceivingChannel
    {
        private static readonly ILogger log = LogManager.GetLogger<RabbitMQReceivingChannel>();
        private EventingBasicConsumer consumer;
        private string inputQueue;

        public RabbitMQReceivingChannel(IChannelFactory channelFactory) : base(channelFactory)
        {
        }

        public void Initialize(EndpointAddress inputAddress, EndpointAddress errorAddress)
        {
            log.Info("Initializing RabbitMQ receving channel");

            inputQueue = inputAddress.Name;
            DeclareQueue(inputQueue);
            DeclareQueue(errorAddress.Name);
            DeclareExchange(inputQueue);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnReceivedMessage;

            channel.BasicConsume(inputAddress.Name, false, string.Empty, false, false, null, consumer);
        }

        private void OnReceivedMessage(object sender, BasicDeliverEventArgs ea)
        {
            if (OnMessage == null)
            {
                channel.BasicAck(ea.DeliveryTag, false);
                return;
            }

            var body = ea.Body;
            var message = new ChannelMessage(new MemoryStream(body));

            foreach (var header in ea.BasicProperties.Headers)
            {
                var value = Encoding.UTF8.GetString((byte[])header.Value);
                message.AddHeader(header.Key, value);
            }

            OnMessage(message);

            channel.BasicAck(ea.DeliveryTag, false);
        }

        public Action<ChannelMessage> OnMessage { get; set; }
    }
}