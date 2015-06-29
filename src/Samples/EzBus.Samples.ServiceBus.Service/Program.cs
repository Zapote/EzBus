using System;

namespace EzBus.Samples.ServiceBus.Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "EzBus.Samples.Msmq.Service";
            Bus.Start();
            Console.Read();
        }
    }
}
