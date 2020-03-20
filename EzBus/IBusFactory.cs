using Microsoft.Extensions.DependencyInjection;

namespace EzBus
{
    public interface IBusFactory : IBrokerConfig
    {
        IBusFactory AddServices(IServiceCollection services);
        IBusFactory LogLevel();
        IBusFactory NumberOfRetries(int n);
        IBusFactory WorkerThreads(int n);
        IBus Create();
    }

    public interface IBrokerConfig
    {
        IBusFactory AddBroker<T>() where T : IBroker;
    }
}
