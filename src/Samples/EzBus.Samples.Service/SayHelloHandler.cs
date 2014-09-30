using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Service
{
    public class SayHelloHandler : IHandle<SayHello>
    {
        private readonly IOtherDependency otherDependency;

        public SayHelloHandler(IOtherDependency otherDependency)
        {
            if (otherDependency == null) throw new ArgumentNullException("otherDependency");
            this.otherDependency = otherDependency;
        }

        public void Handle(SayHello message)
        {
            Console.WriteLine(message);
            Console.WriteLine(otherDependency.Id);
            Bus.Publish(new ClientGreeted());
        }
    }
}