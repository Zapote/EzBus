using System;

namespace EzBus.Samples.Messages.Events
{
    public class OrderCreated
    {
        public OrderCreated(Guid orderId, int orderNumber)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
        }

        public Guid OrderId { get; private set; }
        public int OrderNumber { get; private set; }
    }
}