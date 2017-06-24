using System;

namespace Msmq.Client.Messages
{
    public class OrderConfirmed
    {
        public Guid OrderId { get; set; }
        public DateTime ConfirmationDate { get; set; }
    }
}