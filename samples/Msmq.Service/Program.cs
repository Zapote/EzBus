using System;

namespace Msmq.Service
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Msmq.Service";

            Bus.Start();

            Console.Read();
        }
    }
}
