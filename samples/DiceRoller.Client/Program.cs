using System;
using System.Threading.Tasks;
using EzBus.RabbitMQ;
using EzBus;

namespace DiceRoller.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiceRoller Client";

            Bus.Configure().UseRabbitMQ();

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter> to send RollTheDice");
                keyInfo = Console.ReadKey();

                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine("sending");
                            Bus.Send("DiceRoller.Service", new RollTheDice { Attempts = 10 });
                }

            }
        }
    }
}