using System;
using EzBus.Samples.Messages.Commands;

namespace EzBus.Samples.RabbitMQ.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RabbitMQ.Client";
            Bus.Start();

            var loop = true;

            while (loop)
            {
                var createOrderCommand = new CreateOrder();
                Bus.Send("EzBus.Samples.RabbitMQ.Service", createOrderCommand);
                Bus.Send("EzBus.Samples.RabbitMQ.PlaceOrderService", new PlaceOrder(createOrderCommand.OrderId));
                var keyInfo = Console.ReadKey();
                loop = keyInfo.Key == ConsoleKey.Enter;
            }
        }
    }
}
