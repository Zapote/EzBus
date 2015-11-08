using EzBus.Config;

namespace EzBus
{
    public interface ISubscriptionManager
    {
        void Initialize(ISubscriptionCollection subscriptions);
        void Subscribe(string subscribingEndpointName);
    }
}
