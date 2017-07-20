using System;

namespace Msmq.Service.Messages
{
    public class ConfirmOrder
    {
        public Guid OrderId { get; set; }
    }
}