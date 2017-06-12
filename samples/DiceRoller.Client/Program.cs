using System;

namespace DiceRoller.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Bus.Start();

            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Press <enter>");
                keyInfo = Console.ReadKey();
                Bus.Send("DiceRoller.Service", new RollTheDice());
            }

            
        }
    }
}