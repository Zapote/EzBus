using System;

namespace EzBus.Samples.Msmq.Service.Fwk
{
    public interface IClientGreeter
    {
        Guid Id { get; }
        void GreetClient(string name);
    }
}