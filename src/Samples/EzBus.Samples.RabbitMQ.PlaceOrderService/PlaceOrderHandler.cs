using System;
using EzBus.Samples.Messages.Commands;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.RabbitMQ.PlaceOrderService
{
    public class PlaceOrderHandler : IHandle<PlaceOrder>
    {
        public void Handle(PlaceOrder message)
        {
            Console.WriteLine($"Order placed {message.OrderId}");
            Bus.Publish(new OrderPlaced(message.OrderId));
        }
    }
}