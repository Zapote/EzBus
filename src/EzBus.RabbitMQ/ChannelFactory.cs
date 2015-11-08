using System.Configuration;
using EzBus.Config;
using EzBus.Logging;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
    class ConnectionBuilder
    {
        private static readonly ILogger log = LogManager.GetLogger<ConnectionBuilder>();
        private static readonly string hostUri;

        static ConnectionBuilder()
        {

            hostUri = ConfigurationManager.AppSettings["HostUri"];
            if (string.IsNullOrEmpty(hostUri))
            {
                hostUri = "amqp://localhost";
                log.Debug($"HostUri not found in AppSettings. Defaulting to {hostUri}");
            }
        }

        public static IConnection GetConnection()
        {
            var factory = new ConnectionFactory
            {
                Uri = hostUri
            };

            log.Debug($"Connecting to RabbitMQ Broker");
            return factory.CreateConnection();
        }
    }

    public class SubscriptionManager : ISubscriptionManager
    {
        public void Initialize(ISubscriptionCollection subscriptions)
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe(string subscribingEndpointName) { }
    }
}