using System;

namespace EzBus.Core
{
    public class WorkerThread
    {
        private readonly IReceivingChannel channel;

        public WorkerThread(IReceivingChannel channel)
        {
            this.channel = channel;
            if (channel == null) throw new ArgumentNullException("channel");
        }

        public void Start()
        {
            //channel.Initialize(new EndpointAddress());
        }
    }
}
