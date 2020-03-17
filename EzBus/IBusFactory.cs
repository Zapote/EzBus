using Microsoft.Extensions.DependencyInjection;

namespace EzBus
{
    public interface IBusFactory
    {
        IBusFactory Address(string s);
        IBusFactory AddServices(IServiceCollection services);
        IBusFactory LogLevel();
        IBusFactory NumberOfRetries(int n);
        IBusFactory WorkerThreads(int n);
        IBus Create();
    }
}
