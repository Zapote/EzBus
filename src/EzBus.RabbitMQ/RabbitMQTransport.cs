using System;

namespace EzBus.RabbitMQ
{
    public class RabbitMQTransport : ITransport
    {
        public RabbitMQTransport(IRabbitMQConfig config, IHost host)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public IRabbitMQConfig Config { get; }
        public IHost Host { get; }
    }
}