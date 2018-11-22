using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EzBus.RabbitMQ.Channels
{
    [CLSCompliant(false)]
    public abstract class RabbitMQChannel
    {
        protected readonly IModel channel;
        protected readonly IChannelFactory channelFactory;

        protected RabbitMQChannel(IChannelFactory channelFactory)
        {
            this.channelFactory = channelFactory;
            this.channel = channelFactory.GetChannel();
        }

        protected IBasicProperties ConstructHeaders(ChannelMessage message)
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

        protected void DeclareQueue(string queueName)
        {
            channel.QueueDeclare(queueName, true, false, false);
        }

        protected void DeclareQueuePassive(string queueName)
        {
            try
            {
                channel.QueueDeclarePassive(queueName);
            }
            catch (OperationInterruptedException ex)
            {
                if (ex.ShutdownReason.ReplyCode != 404) throw;

                var message = $"Queue '{queueName}' does not exist or is not currently available.";
                throw new InvalidOperationException(message, ex);
            }
        }

        protected void DeclareExchange(string exchange, string type = "fanout", bool durable = true)
        {
            channel.ExchangeDeclare(exchange, type, true);
        }

        protected void BindQueue(string queueName, string exchange = "")
        {
            channel.QueueBind(queueName, exchange.ToLower(), string.Empty);
        }
    }
}