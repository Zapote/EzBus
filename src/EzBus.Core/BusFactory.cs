using EzBus.Core.Logging;
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
            HostLogManager.Configure(new TraceHostLoggerFactory(), LogLevel.Debug);
            sendingChannel = MessageChannelResolver.GetSendingChannel();
            subscriptionStorage = SubscriptionStorageResolver.GetSubscriptionStorage();
            config.ObjectFactory.Register(subscriptionStorage, LifeCycle.Singleton);

            host = new Host(config);
            host.Start();
            return CreateBus();
        }

        private CoreBus CreateBus()
        {
            return new CoreBus(sendingChannel, new ConfigurableMessageRouting(), subscriptionStorage);
        }
    }
}