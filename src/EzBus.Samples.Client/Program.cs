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
            private readonly Dependency dependency;

            public SayHelloHandler2(Dependency dependency)
            {
                if (dependency == null) throw new ArgumentNullException("dependency");
                this.dependency = dependency;
            }

            public void Handle(SayHello message)
            {
                throw new Exception("Error in handler");
                dependency.PrintOnScreen("Hello {0}.", message.Name);
            }
        }

        public interface IDependency
        {
            void PrintOnScreen(string message, params object[] arg);
        }

        public class Dependency : IDependency
        {
            public void PrintOnScreen(string message, params object[] arg)
            {
                Console.WriteLine(message, arg);
            }
        }
    }
}
