using EzBus.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EzBus.Core
{
    public class BusFactory : IBusFactory
    {
        private readonly Config config = new Config();
        private readonly IServiceCollection services = new ServiceCollection();

        public static IBusFactory Configure(string address = null) => new BusFactory(address);

        public BusFactory(string address)
        {
            if (address.HasValue())
            {
                config.SetAddress(address);
            }

            services.AddSingleton<IBus, Bus>();
        }

        public IBusFactory AddServices(IServiceCollection services)
        {
            foreach(var item in services)
            {
                this.services.Add(item);
            }

            return this;
        }

        public IBusFactory WorkerThreads(int n)
        {
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
            var sp  = services.BuildServiceProvider();
            return sp.GetService<IBus>();
        }

        public IBusFactory Broker(IMessageBroker b)
        {
            return this;
        }
    }
}
