using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private readonly IConfig conf;
        private readonly string endpointName = "TODO";
        private IConnection connection;

        public ChannelFactory(IConfig conf)
        {
            this.conf = conf;
            CreateConnection();
        }

        public IModel GetChannel()
        {
            var channel = connection.CreateModel();
            channel.BasicQos(0, conf.PrefetchCount, false);
            return channel;
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = conf.AutomaticRecoveryEnabled,
                RequestedHeartbeat = conf.RequestedHeartbeat,
                UseBackgroundThreadsForIO = true,
                UserName = conf.UserName,
                Password = conf.Password,
                HostName = conf.HostName,
                VirtualHost = conf.VirtualHost,
                Port = conf.Port
            };

            connection = factory.CreateConnection($"EzBus-{endpointName}");
        }
    }
}