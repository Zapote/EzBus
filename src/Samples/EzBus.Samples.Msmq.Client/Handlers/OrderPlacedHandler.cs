using System;

namespace EzBus.Samples.Msmq.Client.Handlers
{
    public class OrderPlacedHandler : IHandle<OrderPlaced>
    {
        public void Handle(OrderPlaced message)
        {
            Console.WriteLine($"Order {message.OrderId} placed!");
        }
    }

    public class OrderPlaced
    {
        public Guid OrderId { get; set; }
    }
}