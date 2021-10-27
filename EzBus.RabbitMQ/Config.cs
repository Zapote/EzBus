using System;

namespace EzBus.RabbitMQ
{
    internal class Config : IConfig
    {
        public int Port { get; set; } = 5672;
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public TimeSpan RequestedHeartbeat { get; set; } = TimeSpan.FromSeconds(30);
        public ushort PrefetchCount { get; set; } = 100;
        public string ExchangeType { get; set; } = "topic";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string HostName { get; set; } = "localhost";
        public string VirtualHost { get; set; } = "";
    }
}
