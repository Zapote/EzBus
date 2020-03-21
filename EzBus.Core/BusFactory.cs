using System;
using EzBus.Core.Serializers;
using EzBus.Core.Utils;
using EzBus.Serializers;
using EzBus.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EzBus.Core
{
  public class BusFactory : IBusFactory
  {
    private readonly BusConfig conf;
    private readonly IServiceCollection services = new ServiceCollection();

    public static IBrokerConfig Configure(string address) => new BusFactory(address);

    public BusFactory(string address)
    {
      if (address.HasValue())
      {
        conf = new BusConfig(address);
      }
    }

    public IBusFactory AddServices(IServiceCollection services)
    {
      foreach (var item in services)
      {
        this.services.Add(item);
      }

      return this;
    }

    public IBusFactory AddService<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
      services.AddScoped<TService, TImplementation>();
      return this;
    }

    public IBusFactory WorkerThreads(int n)
    {
      conf.SetNumberOfWorkerThreads(n);
      return this;
    }

    public IBusFactory NumberOfRetries(int n)
    {
      return this;
    }

    public IBusFactory LogLevel(LogLevel level)
    {
      conf.SetLogLevel(level);
      return this;
    }

    public IBus CreateBus()
    {
      services.AddSingleton<IBus, Bus>();
      services.AddSingleton<IPublisher, Bus>();
      services.AddSingleton<ISender, Bus>();
      services.AddSingleton<IBusConfig>(conf);
      services.AddSingleton<IAddressConfig>(conf);
      services.AddSingleton<IWorkerThreadsConfig>(conf);
      services.AddSingleton<ITaskRunner, TaskRunner>();
      services.AddSingleton<IHandlerCache, HandlerCache>();
      services.AddSingleton<IBodySerializer, JsonBodySerializer>();
      services.AddSingleton<IAssemblyFinder, AssemblyFinder>();
      services.AddSingleton<IAssemblyScanner, AssemblyScanner>();

      services.AddScoped<IHandlerInvoker, HandlerInvoker>();
      services.AddScoped<ISystemStartupTask, StartBroker>();
      services.AddScoped<ISystemStartupTask, StartConsumers>();

      services.AddLogging(configure => configure.SetMinimumLevel(conf.LogLevel));

      AddStartupTasks();
      AddMiddlewares();
      AddHandlers();

      var serviceProvider = services.BuildServiceProvider();
      return serviceProvider.GetService<IBus>();
    }

    private void AddHandlers()
    {
      var scanner = services.BuildServiceProvider().GetService<IAssemblyScanner>();
      var handlerTypes = scanner.FindTypes<IMessageHandler>();
      foreach (var type in handlerTypes)
      {
        if (type.IsInterface()) continue;
        services.AddScoped(type);
      }
    }

    private void AddStartupTasks()
    {
      var scanner = services.BuildServiceProvider().GetService<IAssemblyScanner>();
      var types = scanner.FindTypes<IStartupTask>();
      foreach (var type in types)
      {
        if (type.IsInterface()) continue;
        services.AddSingleton(typeof(IStartupTask), type);
      }
    }

    private void AddMiddlewares()
    {
      var scanner = services.BuildServiceProvider().GetService<IAssemblyScanner>();
      var types = scanner.FindTypes<IMiddleware>();
      foreach (var type in types)
      {
        if (type.IsInterface()) continue;
        services.AddScoped(typeof(IMiddleware), type);
      }
    }

    public IBusFactory AddBroker<T>() where T : IBroker
    {
      services.AddSingleton(typeof(IBroker), typeof(T));
      return this;
    }
  }
}
