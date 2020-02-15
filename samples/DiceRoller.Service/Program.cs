using System;
using EzBus.Logging;
using EzBus.RabbitMQ;
using Serilog;

namespace DiceRoller.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.ColoredConsole()
            .CreateLogger();

            Console.Title = "DiceRoller Service";
            Bus.Configure(x => x.LogLevel = LogLevel.Debug).UseRabbitMQ(x => x.HostName = "127.0.0.1");

            Console.Read();
        }
    }
}