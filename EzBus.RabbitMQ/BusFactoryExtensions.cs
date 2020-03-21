using Microsoft.Extensions.DependencyInjection;
using System;

namespace EzBus.RabbitMQ
{
    public static class BusFactoryExtensions
    {
        public static IBusFactory UseRabbitMQ(this IBrokerConfig factory, Action<IConfig> action = null)
        {
            var config = new Config();
            action?.Invoke(config);
            var services = new ServiceCollection();
            services.AddSingleton<IConfig>(config);
            services.AddSingleton<IChannelFactory, ChannelFactory>();
            services.AddTransient<IConsumerFactory, ConsumerFactory>();
            return factory.AddBroker<Broker>().AddServices(services);
        }
    }
}