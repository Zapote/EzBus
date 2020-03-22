using System;
using System.Threading.Tasks;
using EzBus.Core;
using EzBus.RabbitMQ;

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

           // await bus.Start();

            Console.Title = "DiceRoller Client";

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter> to send RollTheDice");
                keyInfo = Console.ReadKey();

                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine("sending");
                    await bus.Send("diceroller-worker", new RollTheDice { Attempts = 10 });
                }
            }

            await bus.Stop();
        }
    }
}
