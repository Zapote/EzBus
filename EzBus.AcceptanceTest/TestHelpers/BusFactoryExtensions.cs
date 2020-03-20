using Microsoft.Extensions.DependencyInjection;

namespace EzBus.AcceptanceTest.TestHelpers
{
    public static class BusFactoryExtensions
    {
        public static IBusFactory UseTestBroker(this IBrokerConfig brokerConfig)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConsumer, TestConsumer>();
            return brokerConfig.AddBroker<TestBroker>().AddServices(services);
        }
    }
}