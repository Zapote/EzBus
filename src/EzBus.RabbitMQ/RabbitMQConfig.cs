using System;

namespace EzBus.RabbitMQ
{
    [CLSCompliant(false)]
    public class RabbitMQConfig : IRabbitMQConfig
    {
        public int Port { get; set; } = 5672;
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public ushort RequestedHeartbeat { get; set; } = 5;
        public ushort PrefetchCount { get; set; } = 100;
        public string ExchangeType { get; set; } = "topic";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string HostName { get; set; } = "localhost";
        public string VirutalHost { get; set; } = "/";
    }
}