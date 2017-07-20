using System;

namespace DiceRoller.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiceRoller Service";
            Bus.Start();
        }
    }
}