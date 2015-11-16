using EzBus.Logging;
using EzBus.Samples.Messages.Commands;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.Msmq.Client.Handlers
{
    public class OrderPlacedHandler : IHandle<OrderPlaced>
    {
        private static readonly ILogger log = LogManager.GetLogger<OrderPlaced>();

        public void Handle(OrderPlaced message)
        {
            log.Debug($"Order { message.OrderId} placed!");
        }
    }
}