using System;

namespace EzBus.Samples.RabbitMQ.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RabbitMQ.Service";
            Bus.Start();
        }
    }
}
