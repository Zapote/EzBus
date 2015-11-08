using System;

namespace EzBus.Samples.Messages.Commands
{
    public class CreateOrder
    {
        public CreateOrder()
        {
            OrderId = Guid.NewGuid();
        }

        public Guid OrderId { get; private set; }
    }
}