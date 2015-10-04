using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Msmq.Service.Fwk
{
    public class ClientGreeter : IClientGreeter
    {
        public ClientGreeter()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public void GreetClient(string name)
        {
            Bus.Publish(new Greeting($"Welcome {name}!"));
        }
    }
}