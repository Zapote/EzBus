using EzBus.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EzBus.RabbitMQ
{
    public class Broker : IMessageBroker
    {
        private readonly IChannelFactory channelFactory;
        private readonly IConfig conf;
        private IModel channel;
        private string address;

        public Broker(IChannelFactory channelFactory, IConfig conf)
        {
            this.channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
            this.conf = conf ?? throw new ArgumentNullException(nameof(conf));
        }

        public Task Publish(BasicMessage message)
        {
            return Task.CompletedTask;
        }

        public Task Send(string destination, BasicMessage message)
        {
            QueueDeclarePassive(destination);
            var properties = ConstructHeaders(message);
            var body = message.BodyStream.ToByteArray();

            lock (channel)
            {
                channel.BasicPublish(string.Empty,
                    destination,
                    basicProperties: properties,
                    body: body,
                    mandatory: true);
            }

            return Task.CompletedTask;
        }

        public Task Start(string address, string errorAddress)
        {
            this.address = address;
            channel = channelFactory.GetChannel();

            //log.Info("Initializing RabbitMQ receiving channel");

            channel.QueueDeclare(address, true, false, false);
            channel.QueueDeclare(errorAddress, true, false, false);
            channel.ExchangeDeclare(address, conf.ExchangeType, true);

            return Task.CompletedTask;
        }

        public Task<IConsumer> CreateConsumer()
        {
            return Task.FromResult<IConsumer>(new Consumer(channelFactory, address));
        }

        public Task Stop()
        {
            channelFactory.Close();
            return Task.CompletedTask;
        }

        private IBasicProperties ConstructHeaders(BasicMessage message)
        {
            var props = channel.CreateBasicProperties();

            props.ClearHeaders();
            props.Persistent = true;
            props.Headers = new Dictionary<string, object>();

            foreach (var h in message.Headers)
            {
                props.Headers.Add(h.Name, h.Value);
            }

            return props;
        }

        protected void QueueDeclarePassive(string queueName)
        {
            try
            {
                channel.QueueDeclarePassive(queueName);
            }
            catch (OperationInterruptedException ex)
            {
                if (ex.ShutdownReason.ReplyCode != 404) throw;

                var message = $"Queue '{queueName}' does not exist or is currently not available.";
                throw new InvalidOperationException(message, ex);
            }
        }
    }

    internal class Consumer : IConsumer
    {
        private readonly IChannelFactory factory;
        private readonly string queue;
        private IModel channel;
        private Action<BasicMessage> onMessage;

        public Consumer(IChannelFactory factory, string queue)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.queue = queue;
        }

        public Task Consume(Action<BasicMessage> onMessage)
        {
            this.onMessage = onMessage;
            channel = factory.GetChannel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnReceivedMessage;
            channel.BasicConsume(queue, false, string.Empty, false, false, null, consumer);
            return Task.CompletedTask;
        }

        private void OnReceivedMessage(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = new BasicMessage(new MemoryStream(body));

            foreach (var header in args.BasicProperties.Headers)
            {
                var value = Encoding.UTF8.GetString((byte[])header.Value);
                message.AddHeader(header.Key, value);
            }

            onMessage(message);

            channel.BasicAck(args.DeliveryTag, false);
        }
    }
}
