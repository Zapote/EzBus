using System;

namespace EzBus.RabbitMQ
{
    public class RabbitMQConfig : IRabbitMQConfig
    {
        public int Port { get; set; } = 5672;
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public ushort RequestedHeartbeat { get; set; } = 5;
        public ushort PrefetchCount { get; set; } = 100;
        public string ExchangeType { get; set; } = RabbitMQ.ExchangeType.Topic;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string HostName { get; set; } = "localhost";
        public string VirtualHost { get; set; } = "/";
    }
}