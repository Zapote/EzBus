using EzBus.Core.Resolvers;
using EzBus.Core.Routing;
using EzBus.Logging;

namespace EzBus.Core
{
    public class BusFactory : IBusFactory
    {
        public IBus Start()
        {
            ConfigureLogging();
            return CreateBus();
        }

        private static void ConfigureLogging()
        {
            var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();
            HostLogManager.Configure(loggerFactory, LogLevel.Debug);
        }

        private static CoreBus CreateBus()
        {
            var subscriptionStorage = SubscriptionStorageResolver.GetSubscriptionStorage();
            var sendingChannel = new ChannelResolver().GetSendingChannel();
            return new CoreBus(sendingChannel, new ConfigurableMessageRouting(), subscriptionStorage);
        }
    }
}