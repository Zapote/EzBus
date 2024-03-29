using System;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private readonly IConfig conf;
        private IConnection connection;

        public ChannelFactory(IConfig conf, IAddressConfig addressConf)
        {
            this.conf = conf;
            CreateConnection(addressConf.Address);
        }

        public void Close()
        {
            connection.Close();
            connection.ConnectionShutdown += Connection_ConnectionShutdown;
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }

        public IModel GetChannel()
        {
            var channel = connection.CreateModel();
            channel.BasicQos(0, conf.PrefetchCount, false);
            return channel;
        }

        private void CreateConnection(string name)
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = conf.AutomaticRecoveryEnabled,
                TopologyRecoveryEnabled = true,
                RequestedHeartbeat = conf.RequestedHeartbeat,
                UseBackgroundThreadsForIO = true,
                UserName = conf.UserName,
                Password = conf.Password,
            };

            var scheme = conf.Port == 5671 ? "amqps" : "amqp";
            var uri = new Uri($"{scheme}://{conf.HostName}:{conf.Port}/{conf.VirtualHost}");
            factory.Uri = uri;

            try
            {
                connection = factory.CreateConnection($"EzBus-{name}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create RabbitMQ connection to {uri}.", ex);
            }
        }
    }
}
