using System;

namespace EzBus.RabbitMQ
{
    public static class TransportExtensions
    {
        public static IBusStarter UseRabbitMQ(this ITransport obj, Action<IRabbitMQConfig> action = null)
        {
            if (action == null) return obj.BusStarter;

            var transport = obj as RabbitMQTransport;
            if (transport == null) return obj.BusStarter;

            action(transport.Config);
            return transport.BusStarter;
        }
    }
}