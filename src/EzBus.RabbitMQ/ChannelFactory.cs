using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private readonly IRabbitMQConfig config;
        private IConnection connection;

        public ChannelFactory(IRabbitMQConfig config)
        {
            this.config = config;

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
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                Uri = config.Uri,
                UserName = config.UserName,
                Password = config.Password
            };

            connection = factory.CreateConnection();
        }
    }
}