using System;
using EzBus.Samples.Messages;
using EzBus.Samples.Msmq.Service.Fwk;
using log4net;

namespace EzBus.Samples.Msmq.Service.Handlers
{
    public class SayHelloHandler : IHandle<SayHello>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(SayHelloHandler));
        private readonly IClientGreeter clientGreeter;

        public SayHelloHandler(IClientGreeter clientGreeter)
        {
            if (clientGreeter == null) throw new ArgumentNullException(nameof(clientGreeter));
            this.clientGreeter = clientGreeter;
        }

        public void Handle(SayHello message)
        {
            log.Info(message);
            log.Info(clientGreeter.Id);
            clientGreeter.GreetClient(message.Name);
        }
    }
}