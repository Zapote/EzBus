using System;

namespace EzBus.Samples.Messages.Events
{
    public class OrderPlaced
    {
        public OrderPlaced(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; private set; }
    }
}