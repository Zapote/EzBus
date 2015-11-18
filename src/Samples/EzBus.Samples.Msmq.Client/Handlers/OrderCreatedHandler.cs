using System;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.Msmq.Client.Handlers
{
    public class OrderCreatedHandler : IHandle<OrderCreated>
    {
        public void Handle(OrderCreated message)
        {
            Console.WriteLine($"Order { message.OrderId } successfully created with number { message.OrderNumber }!");
        }
    }
}