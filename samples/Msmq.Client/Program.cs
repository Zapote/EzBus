using System;
using EzBus;

namespace Msmq.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Msmq.Client";

            Bus.Start();

            var orderId = Guid.NewGuid();
            Bus.Send("msmq.service", new ConfirmOrder(orderId));

            Console.WriteLine($"Order confirmation requested. {orderId}");

            Console.Read();
        }
    }

    public class OrderConfirmedHandler : IHandle<OrderConfirmed>
    {
        public void Handle(OrderConfirmed message)
        {
            Console.WriteLine($"Order '{message.OrderId}' confirmed!");
        }
    }
}
