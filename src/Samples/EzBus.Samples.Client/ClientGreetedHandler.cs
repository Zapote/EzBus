using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Client
{
    public class ClientGreetedHandler : IHandle<ClientGreeted>
    {
        public void Handle(ClientGreeted message)
        {
            Console.WriteLine("I am greeted!");
        }
    }
}