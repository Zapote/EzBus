using System;
using EzBus.Samples.Messages;
using EzBus.Samples.Messages.Events;

namespace EzBus.Samples.ServiceBus.Client.Handlers
{
    public class GreetingHandler : IHandle<OrderCreated>
    {
        public void Handle(OrderCreated orderCreated)
        {
            Console.WriteLine("I am greeted! {0}", orderCreated.OrderId);
        }
    }
}