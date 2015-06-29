﻿using System;
using System.Diagnostics;
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
                Bus.Send(new SayHello("Msmq Client"));
                Console.WriteLine("Message sent! Press Enter to send again");
                var keyInfo = Console.ReadKey();
                loop = keyInfo.Key == ConsoleKey.Enter;
            }
        }
    }
}
