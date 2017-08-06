using System;
using EzBus.RabbitMQ;

namespace DiceRoller.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiceRoller Client";

            Bus.Configure().UseRabbitMQ().Start();

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter>");
                keyInfo = Console.ReadKey();
                Bus.Send("DiceRoller.Service", new RollTheDice { Attempts = 10000 });
            }
        }
    }
}