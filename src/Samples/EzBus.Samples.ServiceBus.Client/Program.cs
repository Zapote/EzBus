using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.ServiceBus.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Bus.Start();
            Console.Title = "EzBus.Samples.ServiceBus.Client";
            var loop = true;
            while (loop)
            {
                Bus.Send(new SayHello("Azure ServiceBus Client"));
                Console.WriteLine("Message sent! Press Enter to send again");
                var keyInfo = Console.ReadKey();
                loop = keyInfo.Key == ConsoleKey.Enter;
            }
        }
    }
}
