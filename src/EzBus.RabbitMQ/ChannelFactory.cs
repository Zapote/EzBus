using EzBus.Logging;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private static readonly ILogger log = LogManager.GetLogger<ChannelFactory>();
        private readonly string hostUri;
        private IConnection connection;

        public ChannelFactory()
        {
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