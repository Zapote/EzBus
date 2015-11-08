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

            Bus.SendAsync(new CreateOrder()).ContinueWith(result =>
            {
                Console.WriteLine($"Message sent! Task status: {result.Status}");
                Console.WriteLine("Press Enter to send again");
            });

            //while (loop)
            //{
              

            // //   var keyInfo = Console.ReadKey();
            //   // loop = keyInfo.Key == ConsoleKey.Enter;
            //}

            Console.ReadKey();
        }
    }
}
