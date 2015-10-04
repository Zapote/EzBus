using EzBus.Core.Resolvers;
using EzBus.Core.Routing;
using EzBus.Logging;

namespace EzBus.Core
{
    public class BusFactory : IBusFactory
    {
        public IBus Build()
        {
            return CreateBus();
        }

        private static CoreBus CreateBus()
        {
            var sendingChannel = SendingChannelResolver.GetChannel();
            var publishingChannel = PublishingChannelResolver.GetChannel();
            return new CoreBus(sendingChannel, publishingChannel, new ConfigurableMessageRouting());
        }
    }
}