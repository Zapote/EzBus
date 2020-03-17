using EzBus.Core.Utils;
using EzBus.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EzBus.Core
{
    public class BusFactory : IBusFactory
    {
        private readonly Config conf = new Config();
        private readonly IServiceCollection services = new ServiceCollection();

        public static IBusFactory Configure(string address = null) => new BusFactory(address);

        public BusFactory(string address)
        {
            if (address.HasValue())
            {
                conf.SetAddress(address);
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
            services.AddSingleton<IConfig>(conf);
            var sp = services.BuildServiceProvider();
            return sp.GetService<IBus>();
        }
    }
}
