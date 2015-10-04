using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Msmq.Client.Handlers
{
    public class GreetingHandler : IHandle<Greeting>
    {
        public void Handle(Greeting greeting)
        {
            Console.WriteLine($"I am greeted! { greeting.Message}");
        }
    }
}