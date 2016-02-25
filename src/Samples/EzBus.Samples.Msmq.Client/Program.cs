using System;
using EzBus.Samples.Messages.Commands;

namespace EzBus.Samples.Msmq.Client
{
    public class Program
    {
        public static class IHostCustomizationExt
        {
            // public static void UseStructureMap(this IHostCustomization )
        }

        static void Main(string[] args)
        {
            Console.Title = "EzBus.Samples.Msmq.Client";

            Bus.Configure()
                .WorkerThreads(3);
            //UseStructureMap(new Container);

            Bus.Start();

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
