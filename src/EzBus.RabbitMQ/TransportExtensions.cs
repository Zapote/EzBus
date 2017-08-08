using System;

namespace EzBus.RabbitMQ
{
    public static class TransportExtensions
    {
        public static IHost UseRabbitMQ(this ITransport obj, Action<IRabbitMQConfig> action = null)
        {
            if (action == null) return obj.Host;

            var transport = obj as RabbitMQTransport;
            if (transport == null) return obj.Host;

            action(transport.Config);
            return transport.Host;
        }
    }
}