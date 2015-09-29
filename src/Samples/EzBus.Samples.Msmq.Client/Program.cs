using System;
using EzBus.Samples.Messages;

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
                Bus.SendAsync(new SayHello("Msmq Client")).ContinueWith(r => Console.WriteLine("message sent {0}", r.Status));

                Console.WriteLine("Press Enter to send again");
                var keyInfo = Console.ReadKey();
                loop = keyInfo.Key == ConsoleKey.Enter;

            }
        }
    }
}
