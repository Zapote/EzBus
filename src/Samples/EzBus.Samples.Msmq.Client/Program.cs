using System;
using EzBus.Samples.Messages;
using EzBus.Samples.Messages.Commands;

namespace EzBus.Samples.Msmq.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Bus.Start();

            Console.Title = "EzBus.Samples.Msmq.Client";
            var loop = true;

            while (loop)
            {
                Bus.Send(new CreateOrder());

                Console.WriteLine("CreateOrder sent.");
                Console.WriteLine("Press Enter to send again");

                var keyInfo = Console.ReadKey();
                loop = keyInfo.Key == ConsoleKey.Enter;
            }

            Console.ReadKey();
        }
    }
}
