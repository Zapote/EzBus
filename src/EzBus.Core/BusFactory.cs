using EzBus.Core.Resolvers;
using EzBus.Core.Routing;
using EzBus.Logging;

namespace EzBus.Core
{
    public class BusFactory : IBusStarter
    {
        private readonly HostConfig config = new HostConfig();
        private Host host;
        private ISendingChannel sendingChannel;
        private ISubscriptionStorage subscriptionStorage;

        public IBus Start()
        {
            GetSendingChannel();
            ConfigureLogging();
            ConfigureSubscriptionStorage();
            StartHost();
            return CreateBus();
        }

        private void GetSendingChannel()
        {
            sendingChannel = MessageChannelResolver.GetSendingChannel();
        }

        private void StartHost()
        {
            host = new Host(config);
            host.Start();
        }

        private void ConfigureSubscriptionStorage()
        {
            subscriptionStorage = SubscriptionStorageResolver.GetSubscriptionStorage();
            config.ObjectFactory.Register(subscriptionStorage, LifeCycle.Singleton);
        }

        private static void ConfigureLogging()
        {
            var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();
            HostLogManager.Configure(loggerFactory, LogLevel.Debug);
        }

        private CoreBus CreateBus()
        {
            return new CoreBus(sendingChannel, new ConfigurableMessageRouting(), subscriptionStorage);
        }
    }
}