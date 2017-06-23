using System;
using EzBus;

namespace Msmq.Service
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Msmq.Service";

            Bus.Start();

            Console.Read();
        }

        public class ConfirmOrder
        {
            public Guid OrderId { get; set; }
        }

        public class OrderConfirmed
        {
            public Guid OrderId { get; set; }
            public DateTime ConfirmationDate { get; set; }
        }

        public class ConfirmOrderHandler : IHandle<ConfirmOrder>
        {
            public void Handle(ConfirmOrder message)
            {
                Console.WriteLine($"Order '{message.OrderId}' confirmed!");
                Bus.Publish(new OrderConfirmed
                {
                    OrderId = message.OrderId,
                    ConfirmationDate = DateTime.Now
                });
            }
        }
    }
}
