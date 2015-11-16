namespace EzBus
{
    public interface IHostConfig
    {
        int WorkerThreads { get; }
        int NumberOfRetrys { get; }
        string EndpointName { get; }
        string ErrorEndpointName { get; }
    }
}