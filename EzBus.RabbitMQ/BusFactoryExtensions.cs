using Microsoft.Extensions.DependencyInjection;
using System;

namespace EzBus.RabbitMQ
{
    public static class BusFactoryExtensions
    {
        public static IBusFactory UseRabbitMQ(this IBusFactory factory, Action<IConfig> action = null)
        {
            var config = new Config();
            action?.Invoke(config);

            var services = new ServiceCollection();
            services.AddSingleton<IConfig>(config);
            services.AddSingleton<IChannelFactory, ChannelFactory>();
            services.AddSingleton<IBroker, Broker>();

            return factory.AddServices(services);
        }
    }
}