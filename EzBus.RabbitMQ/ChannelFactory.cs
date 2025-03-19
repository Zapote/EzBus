using System;
using System.Dynamic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EzBus.RabbitMQ
{
    internal class ChannelFactory : IChannelFactory
    {
        private readonly IConfig conf;
        private IConnection connection;

        public ChannelFactory(IConfig conf, IAddressConfig addressConf)
        {
            this.conf = conf;
            Task.WaitAll(CreateConnection(addressConf.Address));
        }

        public async Task Close()
        {
            await connection.CloseAsync();
            connection.ConnectionShutdownAsync += ShutdownConnection;
        }

        private async Task ShutdownConnection(object sender, ShutdownEventArgs @event)
        {
            await connection.DisposeAsync();
        }

        public async Task<IChannel> GetChannel()
        {
            var channel = await connection.CreateChannelAsync();
            await channel.BasicQosAsync(0, conf.PrefetchCount, false);
            return channel;
        }

        private async Task CreateConnection(string name)
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = conf.AutomaticRecoveryEnabled,
                TopologyRecoveryEnabled = true,
                RequestedHeartbeat = conf.RequestedHeartbeat,
                UserName = conf.UserName,
                Password = conf.Password,
            };

            var scheme = conf.Port == 5671 ? "amqps" : "amqp";
            var uri = new Uri($"{scheme}://{conf.HostName}:{conf.Port}/{conf.VirtualHost}");
            factory.Uri = uri;

            try
            {
                connection = await factory.CreateConnectionAsync($"EzBus-{name}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create RabbitMQ connection to {uri}.", ex);
            }
        }
    }
}
