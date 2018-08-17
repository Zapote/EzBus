using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private readonly IRabbitMQConfig rabbitCfg;
        private readonly string endpointName;
        private IConnection connection;

        public ChannelFactory(IRabbitMQConfig rabbitCfg, IBusConfig busCfg)
        {
            this.rabbitCfg = rabbitCfg;
            endpointName = busCfg.EndpointName;
            CreateConnection();
        }

        public IModel GetChannel()
        {
            var channel = connection.CreateModel();
            channel.BasicQos(0, rabbitCfg.PrefetchCount, false);
            return channel;
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = rabbitCfg.AutomaticRecoveryEnabled,
                RequestedHeartbeat = rabbitCfg.RequestedHeartbeat,
                UseBackgroundThreadsForIO = true,
                UserName = rabbitCfg.UserName,
                Password = rabbitCfg.Password,
                HostName = rabbitCfg.HostName,
                VirtualHost = rabbitCfg.VirutalHost,
                Port = rabbitCfg.Port
            };

            connection = factory.CreateConnection($"EzBus - {endpointName}");
        }
    }
}