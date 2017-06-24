using System;

namespace Msmq.Client.Messages
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