using System;
using EzBus.Core;
using EzBus.Msmq;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bus = BusFactory.Setup().WithMsmq().Start();
            bus.Send(new SayHello("Larry"));
            Console.Read();
        }

        public class SayHelloHandler : IHandle<SayHello>
        {
            public void Handle(SayHello message)
            {
                Console.WriteLine("Hello {0}! This is 1 ez!", message.Name);
            }
        }

        public class SayHelloHandler2 : IHandle<SayHello>
        {
            public void Handle(SayHello message)
            {
                Console.WriteLine("Hello {0}! This is 2 ez!", message.Name);
                //throw new Exception("UNABLE TO FIX!");
            }
        }
    }
}
