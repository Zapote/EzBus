using System;
using EzBus.Samples.Messages;
using log4net;

namespace EzBus.Samples.Service
{
    public class SayHelloHandler : IHandle<SayHello>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(SayHelloHandler));
        private readonly IOtherDependency otherDependency;

        public SayHelloHandler(IOtherDependency otherDependency)
        {
            if (otherDependency == null) throw new ArgumentNullException("otherDependency");
            this.otherDependency = otherDependency;
        }

        public void Handle(SayHello message)
        {
            log.Debug(message);
            log.Debug(otherDependency.Id);
            Bus.Publish(new ClientGreeted());
        }
    }
}