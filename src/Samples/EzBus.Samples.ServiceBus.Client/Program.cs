using System;
using EzBus.Samples.Messages;
using EzBus.Samples.Messages.Commands;

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
                Bus.Send(new CreateOrder());
                Console.WriteLine("Message sent! Press Enter to send again");
                var keyInfo = Console.ReadKey();
                loop = keyInfo.Key == ConsoleKey.Enter;
            }
        }
    }
}
