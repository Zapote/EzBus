using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EzBus
{
  public interface IBusFactory : IBrokerConfig
  {
    IBusFactory AddServices(IServiceCollection services);
    IBusFactory AddService<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
    IBusFactory LogLevel(LogLevel level);
    IBusFactory NumberOfRetries(int n);
    IBusFactory WorkerThreads(int n);
    IBus CreateBus();
  }

  public interface IBrokerConfig
  {
    IBusFactory AddBroker<T>() where T : IBroker;
  }
}
