using System;

namespace EzBus.RabbitMQ
{
    [CLSCompliant(false)]
    public class RabbitMQConfig : IRabbitMQConfig
    {
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public ushort RequestedHeartbeat { get; set; } = 5;
        public string Uri { get; set; } = "amqp://localhost";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}