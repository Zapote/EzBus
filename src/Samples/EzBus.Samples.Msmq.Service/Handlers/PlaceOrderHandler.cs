using EzBus.Samples.Messages.Commands;
using EzBus.Samples.Messages.Events;
using log4net;

namespace EzBus.Samples.Msmq.Service.Handlers
{
    public class PlaceOrderHandler : IHandle<PlaceOrder>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PlaceOrderHandler));

        public void Handle(PlaceOrder message)
        {
            log.Debug($"Order {message.OrderId} placed!");

            Bus.Publish(new OrderPlaced(message.OrderId));
        }
    }
}