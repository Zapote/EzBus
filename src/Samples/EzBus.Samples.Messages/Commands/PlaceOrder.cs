using System;

namespace EzBus.Samples.Messages.Commands
{
    public class PlaceOrder
    {
        public PlaceOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; private set; }
    }
}
