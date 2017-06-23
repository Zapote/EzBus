using System;

namespace Msmq.Client
{
    public class ConfirmOrder
    {
        public ConfirmOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}