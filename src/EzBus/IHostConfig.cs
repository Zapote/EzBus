namespace EzBus
{
    public interface IHostConfig
    {
        ISendingChannel SendingChannel { get; }
        IReceivingChannel ReceivingChannel { get; }
        int WorkerThreads { get; }
        void SetSendingChannel(ISendingChannel sender);
        void SetReceivingChannel(IReceivingChannel receiver);
        void SetNumberOfWorkerThreads(int threads);
    }
}