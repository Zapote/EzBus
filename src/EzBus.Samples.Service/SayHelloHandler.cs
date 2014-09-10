using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Service
{
    public class SayHelloHandler : IHandle<SayHello>
    {
        public void Handle(SayHello message)
        {
            Console.WriteLine(message);
        }
    }
}