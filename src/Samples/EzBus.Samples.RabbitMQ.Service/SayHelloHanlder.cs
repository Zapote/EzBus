using System;
using EzBus.Samples.Messages.Commands;

namespace EzBus.Samples.RabbitMQ.Service
{
    public class SayHelloHanlder : IHandle<CreateOrder>
    {
        public void Handle(CreateOrder message)
        {
            Console.WriteLine($"Hello {message.OrderId }");
        }
    }
}