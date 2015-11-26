using System;

namespace EzBus.Samples.RabbitMQ.PlaceOrderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RabbitMQ.PlaceOrderService";
            Bus.Start();
        }
    }
}
