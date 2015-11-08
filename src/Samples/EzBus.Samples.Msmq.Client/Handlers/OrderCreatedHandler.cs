using EzBus.Logging;
using EzBus.Samples.Messages.Commands;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.Msmq.Client.Handlers
{
    public class OrderCreatedHandler : IHandle<OrderCreated>
    {
        private static readonly ILogger log = LogManager.GetLogger<OrderCreatedHandler>();

        public void Handle(OrderCreated message)
        {
            log.Debug($"Order { message.OrderNumber} successfully!");
            log.Debug("Placing order");

            Bus.Send(new PlaceOrder(message.OrderId));
        }
    }
}