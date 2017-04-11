using EzBus.Core.Config;

namespace EzBus.Config
{
    public interface IEzBusConfig
    {
        string EndpointName { get; set; }
        Destination[] Destinations { get; set; }
        Subscription[] Subscriptions { get; set; }
    }
}