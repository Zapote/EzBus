using System;
using System.Threading.Tasks;
using EzBus.Core;
using EzBus.RabbitMQ;
using System.Diagnostics;

namespace DiceRoller.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = BusFactory
                .Address("diceroller-client")
                .UseRabbitMQ()
                .CreateBus();

            Console.Title = "DiceRoller Client";

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter> to send RollTheDice");
                keyInfo = Console.ReadKey();


                Console.WriteLine("sending");
                try
                {
                    var stopwatch = new Stopwatch();

                    stopwatch.Start();
                    await bus.Send("diceroller-worker", new RollTheDice { Attempts = 10 });
                    stopwatch.Stop();
                    System.Console.WriteLine("Outer:" + stopwatch.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }

            await bus.Stop();
        }
    }
}
