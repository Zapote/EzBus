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
        /// <summary>
        /// Heartbeat setting for connection in seconds. 0 for disabled
        /// </summary>
        ushort RequestedHeartbeat { get; set; }
        /// <summary>
        /// Maximum of unacknowledged messages at once. Default 100
        /// </summary>
        ushort PrefetchCount { get; set; }
    }
}