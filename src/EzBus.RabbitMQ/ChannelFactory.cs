using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private readonly IRabbitMQConfig config;
        private readonly string endpointName;
        private IConnection connection;


        public ChannelFactory(IRabbitMQConfig config, IBusConfig busConfig)
        {
            this.config = config;
            endpointName = busConfig.EndpointName;
            CreateConnection();
        }

        public IModel GetChannel()
        {
            var channel = connection.CreateModel();
            connection.AutoClose = true;
            return channel;
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                RequestedHeartbeat = config.RequestedHeartbeat,
                UseBackgroundThreadsForIO = true,
                Uri = config.Uri,
                UserName = config.UserName,
                Password = config.Password
            };

            connection = factory.CreateConnection($"EzBus - {endpointName}");
        }
    }
}