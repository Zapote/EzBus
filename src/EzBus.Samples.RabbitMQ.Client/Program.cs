using System;
using EzBus.Samples.Messages;
using EzBus.Samples.Messages.Commands;

namespace EzBus.Samples.RabbitMQ.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RabbitMQ.Client";
            Bus.Send("EzBus.Samples.RabbitMQ.Service", new CreateOrder());
        }
    }
}
