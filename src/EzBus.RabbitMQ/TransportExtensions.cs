using System;

namespace EzBus.RabbitMQ
{
    public static class TransportExtensions
    {
        public static void UseRabbitMQ(this ITransport obj, Action<IRabbitMQConfig> action = null)
        {
            var transport = obj as RabbitMQTransport;

            if (transport == null)
            {
                throw new Exception("Failed to setup RabbitMQ transport");
            }

            action?.Invoke(transport.Config);
            transport.Host.Start();
        }
    }
}