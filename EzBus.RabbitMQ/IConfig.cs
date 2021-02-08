using System;

namespace EzBus.RabbitMQ
{
    public interface IConfig
    {
        /// <summary>
        /// Username to use when authenticating to the server
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// Password to use when authenticating to the server
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// The host to connect to
        /// </summary>
        string HostName { get; set; }
        /// <summary>
        /// Virtual host to access during this connection
        /// </summary>
        string VirtualHost { get; set; }
        /// <summary>
        /// The port to connect on
        /// </summary>
        int Port { get; set; }
        /// <summary>
        /// Set to false to disable automatic recovery.
        /// </summary>
        bool AutomaticRecoveryEnabled { get; set; }
        /// <summary>
        /// Heartbeat setting for connection in seconds. 0 for disabled.
        /// </summary>
        TimeSpan RequestedHeartbeat { get; set; }
        /// <summary>
        /// Maximum of unacknowledged messages at once. Default 100
        /// </summary>
        ushort PrefetchCount { get; set; }
        /// <summary>
        /// ExchangeType for publishing. Direct, Fanout or Topic. Defaults to Topic.
        /// </summary>
        string ExchangeType { get; set; }
    }
}