using EzBus.Logging;

namespace EzBus
{
    public interface IBusConfig : IAddressConfig, IRetryConfig, IWorkerThreadsConfig, ILogLevelConfig
    {
        
    }

    public interface IAddressConfig
    {
        string Address { get; }
        string ErrorAddress { get; }
    }

    public interface IRetryConfig
    {
        int NumberOfRetries { get; }
    }

    public interface IWorkerThreadsConfig
    {
        int WorkerThreads { get; }
    }

    public interface ILogLevelConfig
    {
        LogLevel LogLevel { get; }
    }
}