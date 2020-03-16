using System;
using EzBus.Core;
using EzBus.RabbitMQ;

namespace DiceRoller.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = BusFactory
                .Configure()
                .UseRabbitMQ()
                .Create();

            bus.Start().GetAwaiter().GetResult();
            
            bus.Send("diceroller.service", new RollTheDice { Attempts = 10 });

            
            //bus.Stop();


            Console.Title = "DiceRoller Client";

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter> to send RollTheDice");
                keyInfo = Console.ReadKey();

                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine("sending");
                    bus.Send("diceroller.service", new RollTheDice { Attempts = 10 });
                }

            }
        }
    }
}