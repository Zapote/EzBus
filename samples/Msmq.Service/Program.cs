using System;
using EzBus;

namespace Msmq.Service
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Msmq.Service";

            Bus.Configure().UseMsmq();

            Console.Read();
        }
    }
}
