using System;
using EzBus.RabbitMQ;

namespace DiceRoller.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiceRoller Service";
            Bus.Configure().UseRabbitMQ().Start();
        }
    }
}