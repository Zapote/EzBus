using System;
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
            var channel = connection.CreateModel();
            connection.AutoClose = true;
            return channel;
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