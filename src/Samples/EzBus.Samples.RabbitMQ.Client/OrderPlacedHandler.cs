using System;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.RabbitMQ.Client
{
    public class OrderPlacedHandler : IHandle<OrderPlaced>
    {
        public void Handle(OrderPlaced message)
        {
            Console.WriteLine($"{DateTime.Now.ToLocalTime()}: Order placed: {message.OrderId}");
        }
    }
}