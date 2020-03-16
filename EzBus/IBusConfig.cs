using EzBus.Logging;

namespace EzBus
{
    public interface IConfig
    {
        int WorkerThreads { get; }
        int NumberOfRetries { get; }
        string Address { get; }
        string ErrorAddress { get; }
        LogLevel LogLevel { get; }
    }
}