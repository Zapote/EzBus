﻿using EzBus.Core.Builders;

namespace EzBus.Core
{
    public class HostConfig : IHostConfig
    {
        public ISendingChannel SendingChannel { get; private set; }
        public IReceivingChannel ReceivingChannel { get; private set; }
        public IObjectFactory ObjectFactory { get; private set; }
        public int WorkerThreads { get; private set; }
        public int NumberOfRetrys { get; private set; }

        public HostConfig()
        {
            NumberOfRetrys = 5;
            ObjectFactory = new DefaultObjectFactory();
        }

        public void SetSendingChannel(ISendingChannel sender)
        {
            SendingChannel = sender;
        }

        public void SetReceivingChannel(IReceivingChannel receiver)
        {
            ReceivingChannel = receiver;
        }

        public void SetNumberOfWorkerThreads(int threads)
        {
            WorkerThreads = threads;
        }
    }
}