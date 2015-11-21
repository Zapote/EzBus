using System;
using EzBus.Samples.Messages.Commands;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.RabbitMQ.Service
{
    public class CreateOrderHandler : IHandle<CreateOrder>
    {
        public void Handle(CreateOrder message)
        {
            Console.WriteLine($"Order created {message.OrderId }");
            Bus.Publish(new OrderCreated(message.OrderId, 2));
        }
    }
}