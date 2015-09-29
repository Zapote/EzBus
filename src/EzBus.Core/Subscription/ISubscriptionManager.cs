using System.Xml.Serialization;

namespace EzBus.Core.Subscription
{
    public interface ISubscriptionManager
    {
        void Subscribe(string endpointName);
    }
}
