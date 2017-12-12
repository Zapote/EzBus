using System;

namespace EzBus.RabbitMQ
{
    public static class TransportExtensions
    {
        public static void UseRabbitMQ(this ITransport obj, Action<IRabbitMQConfig> action = null)
        {
            if (!(obj is RabbitMQTransport transport))
            {
                throw new Exception("Failed to setup RabbitMQ transport");
            }

            action?.Invoke(transport.Config);
            transport.Host.Start();
        }
    }
}