using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Msmq.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "EzBus.Samples.Msmq.Client";
            Bus.Send(new SayHello("Client Larry"));
            Console.Read();
        }
    }
}
