using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.ServiceBus.Client.Handlers
{
    public class GreetingHandler : IHandle<Greeting>
    {
        public void Handle(Greeting greeting)
        {
            Console.WriteLine("I am greeted! {0}", greeting.Message);
        }
    }
}