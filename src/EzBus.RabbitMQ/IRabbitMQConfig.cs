using System;

namespace EzBus.RabbitMQ
{
    [CLSCompliant(false)]
    public interface IRabbitMQConfig
    {
        string Uri { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool AutomaticRecoveryEnabled { get; set; }
        ushort RequestedHeartbeat { get; set; }
    }
}