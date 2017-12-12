using System;
using EzBus.Logging;
using EzBus.RabbitMQ;

namespace DiceRoller.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiceRoller Service";
            Bus.Configure(x => x.LogLevel = LogLevel.Debug).UseRabbitMQ(x => x.HostName = "127.0.0.1");

            Console.Read();
        }
    }
}