using System;
using System.Threading.Tasks;
using EzBus.Core;
using EzBus.RabbitMQ;

namespace DiceRoller.Statistics
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = BusFactory
                       .Configure("diceroller-statistics")
                       .UseRabbitMQ()
                       .CreateBus();

            await bus.Start();

            Console.Title = "DiceRoller Statistics";
            Console.Read();
        }
    }
}