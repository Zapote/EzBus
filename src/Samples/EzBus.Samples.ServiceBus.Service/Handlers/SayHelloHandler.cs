using System;
using EzBus.Samples.Messages.Commands;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.ServiceBus.Service.Handlers
{
    public class CreateOrderHandler : IHandle<CreateOrder>
    {
        public void Handle(CreateOrder message)
        {
            Console.WriteLine("Order created {0}", message.OrderId);
            Bus.Publish(new OrderCreated(message.OrderId, 1));
        }
    }
}