namespace EzBus.Core
{
    public class EndpointConfig : IEndpointConfig
    {
        public ISendingChannel SendingChannel { get; private set; }
        public IReceivingChannel ReceivingChannel { get; private set; }
        public int WorkerThreads { get; private set; }

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