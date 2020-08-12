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

            Console.Title = "DiceRoller Client";

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter> to send RollTheDice");
                keyInfo = Console.ReadKey();


                Console.WriteLine("sending");
                try
                {
                    await bus.Send("diceroller-worker", new RollTheDice { Attempts = 10 });
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
