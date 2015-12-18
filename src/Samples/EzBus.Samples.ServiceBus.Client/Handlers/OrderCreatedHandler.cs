using System;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.ServiceBus.Client.Handlers
{
    public class OrderCreatedHandler : IHandle<OrderCreated>
    {
        public void Handle(OrderCreated orderCreated)
        {
            Console.WriteLine("Order created! {0}", orderCreated.OrderId);
        }
    }
}