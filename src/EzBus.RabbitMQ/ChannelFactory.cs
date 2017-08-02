using System;
using EzBus.Config;
using EzBus.Logging;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private readonly IEzBusConfig config;
        private static readonly ILogger log = LogManager.GetLogger<ChannelFactory>();
        private readonly string hostUri;
        private IConnection connection;

        public ChannelFactory(IEzBusConfig config, IBusConfig busConfig)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));

            if (string.IsNullOrEmpty(hostUri))
            {
                hostUri = "amqp://localhost";
                log.Warn($"HostUri not found in AppSettings. Defaulting to {hostUri}");
            }

            CreateConnection();
        }

        public IModel GetChannel()
        {
            return connection.CreateModel();
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                Uri = hostUri,
                AutomaticRecoveryEnabled = true
            };
            connection = factory.CreateConnection();
        }
    }
}