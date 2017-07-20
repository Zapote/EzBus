namespace EzBus
{
    public interface IBusConfig
    {
        int WorkerThreads { get; set; }
        int NumberOfRetrys { get; set; }
        string EndpointName { get; set; }
        string ErrorEndpointName { get; set; }
    }
}