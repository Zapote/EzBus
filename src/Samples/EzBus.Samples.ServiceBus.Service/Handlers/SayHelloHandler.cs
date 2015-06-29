using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.ServiceBus.Service.Handlers
{
    public class SayHelloHandler : IHandle<SayHello>
    {
        public void Handle(SayHello message)
        {
            Console.WriteLine("{0} says hello", message.Name);
            Bus.Publish(new Greeting("Welcome " + message.Name));
        }
    }
}