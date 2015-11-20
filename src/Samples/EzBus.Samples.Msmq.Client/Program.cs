using System;
using EzBus.Samples.Messages.Commands;

namespace EzBus.Samples.Msmq.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Bus.Configure()
                .WorkerThreads(3);

            Bus.Start();

            Console.Title = "EzBus.Samples.Msmq.Client";
            var loop = true;

            while (loop)
            {
                var createOrder = new CreateOrder();

                Console.WriteLine("Creating order.");
                Bus.Send(createOrder);

                Console.WriteLine("Placing order.");
                Bus.Send(new PlaceOrder(createOrder.OrderId));

                var keyInfo = Console.ReadKey();
                loop = keyInfo.Key == ConsoleKey.Enter;
            }

            Console.ReadKey();
        }
    }
}
