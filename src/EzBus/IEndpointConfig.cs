namespace EzBus
{
    public interface IEndpointConfig
    {
        ISendingChannel SendingChannel { get; }
        IReceivingChannel ReceivingChannel { get; }
        int WorkerThreads { get; }
        void SetSendingChannel(ISendingChannel sender);
        void SetReceivingChannel(IReceivingChannel receiver);
        void SetNumberOfWorkerThreads(int threads);
    }
}