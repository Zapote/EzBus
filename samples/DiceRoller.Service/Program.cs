using System;
using System.Threading.Tasks;
using EzBus.Core;
using EzBus.Logging;
using EzBus.RabbitMQ;
using Serilog;

namespace DiceRoller.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.ColoredConsole()
            .CreateLogger();

            var bus = BusFactory
                        .Configure()
                        .UseRabbitMQ()
                        .Create();

            await bus.Start();

            Console.Title = "DiceRoller Service";
            //Bus.Configure(x => x.LogLevel = LogLevel.Debug).UseRabbitMQ(x => x.HostName = "127.0.0.1");

            //BusFactory.Create


            Console.Read();
        }
    }
}
