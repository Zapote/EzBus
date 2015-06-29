using EzBus.Core.Resolvers;

namespace EzBus.Core
{
    public class HostFactory
    {
        public Host Build()
        {
            var hostConfig = new HostConfig();
            var subscriptionStorage = SubscriptionStorageResolver.GetSubscriptionStorage();
            hostConfig.ObjectFactory.Register(subscriptionStorage, LifeCycle.Singleton);
            return new Host(hostConfig);
        }
    }
}