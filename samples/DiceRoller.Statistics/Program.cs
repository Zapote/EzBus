using System;
using System.Threading.Tasks;
using EzBus.Core;
using EzBus.RabbitMQ;
using Microsoft.Extensions.Logging;

namespace DiceRoller.Statistics
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = BusFactory
                       .Address("diceroller-statistics")
                       .UseRabbitMQ()
                       .LogLevel(LogLevel.Debug)
                       .CreateBus();
            
            await bus.Subscribe("diceroller-worker", "DiceRolled");
            await bus.Start();
            

            

            Console.Title = "DiceRoller Statistics";
            Console.Read();

            await bus.Unsubscribe("diceroller-worker", "DiceRolled");
            await bus.Stop();
        }
    }
}