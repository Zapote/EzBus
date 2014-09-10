using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Bus.Send(new SayHello("Client Larry"));
            Console.Read();
        }
    }
}
