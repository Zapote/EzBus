using System;

namespace EzBus.RabbitMQ
{
    public class RabbitMQTransport : ITransport
    {
        public RabbitMQTransport(IRabbitMQConfig config, IBusStarter busStarter)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            BusStarter = busStarter ?? throw new ArgumentNullException(nameof(busStarter));
        }

        public IRabbitMQConfig Config { get; }
        public IBusStarter BusStarter { get; }
    }
}