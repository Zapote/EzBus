using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EzBus.RabbitMQ.Channels
{
    public abstract class RabbitMQChannel
    {
        protected readonly IModel channel;

        protected RabbitMQChannel(IChannelFactory channelFactory)
        {
            channel = channelFactory?.GetChannel() ?? throw new ArgumentNullException(nameof(channelFactory));
        }

        protected IBasicProperties ConstructHeaders(ChannelMessage channelMessage)
        {
            var properties = channel.CreateBasicProperties();

            properties.ClearHeaders();
            properties.Persistent = true;
            properties.Headers = new Dictionary<string, object>();
            foreach (var header in channelMessage.Headers)
            {
                properties.Headers.Add(header.Name, header.Value);
            }

            return properties;
        }

        protected void DeclareQueue(string queueName)
        {
            channel.QueueDeclare(queueName, true, false, false, null);
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

                string message = $"Queue '{queueName}' does not exist or is not currently available.";
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