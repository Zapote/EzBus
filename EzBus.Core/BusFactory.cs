using EzBus.Core.Serializers;
using EzBus.Core.Utils;
using EzBus.Serializers;
using EzBus.Utils;
using Microsoft.Extensions.DependencyInjection;

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

            var scanner = new AssemblyScanner();
            var handlerTypes = scanner.FindTypes<IMessageHandler>();
            foreach (var type in handlerTypes)
            {
                if (type.IsInterface()) continue;
                services.AddScoped(type);
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

        public IBusFactory WorkerThreads(int n)
        {
            conf.SetNumberOfWorkerThreads(n);
            return this;
        }

        public IBusFactory NumberOfRetries(int n)
        {
            return this;
        }

        public IBusFactory LogLevel()
        {
            return this;
        }

        public IBusFactory Address(string s)
        {
            return this;
        }

        public IBus Create()
        {
            services.AddSingleton<IBus, Bus>();
            services.AddSingleton<IPublisher, Bus>();
            services.AddSingleton<ISender, Bus>();
            services.AddSingleton<IBusConfig>(conf);
            services.AddSingleton<IAddressConfig>(conf);
            services.AddSingleton<ITaskRunner, TaskRunner>();
            services.AddSingleton<IConsumerFactory, ConsumerFactory>();
            services.AddSingleton<IHandlerCache, HandlerCache>();
            services.AddSingleton<IBodySerializer, JsonBodySerializer>();

            services.AddScoped<IHandlerInvoker, HandlerInvoker>();
            services.AddScoped<ISystemStartupTask, StartBroker>();
            services.AddScoped<ISystemStartupTask, StartConsumers>();

            AddStartupTasks(services);
            AddMiddlewares(services);

            var sp = services.BuildServiceProvider();
            return sp.GetService<IBus>();
        }

        private void AddStartupTasks(IServiceCollection services)
        {
            var scanner = new AssemblyScanner();
            var types = scanner.FindTypes<IStartupTask>();
            foreach (var type in types)
            {
                if (type.IsInterface()) continue;
                services.AddScoped(typeof(IStartupTask), type);
            }
        }

        private void AddMiddlewares(IServiceCollection services)
        {
            var scanner = new AssemblyScanner();
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
