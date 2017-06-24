using System;

namespace Msmq.Service.Messages
{
    public class OrderConfirmed
    {
        public Guid OrderId { get; set; }
        public DateTime ConfirmationDate { get; set; }
    }
}