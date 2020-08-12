using System;
using System.Threading.Tasks;

namespace EzBus.AcceptanceTest.TestHelpers
{
  public class TestConsumerFactory : IConsumerFactory
  {
    private readonly IAddressConfig addressConfig;
    private readonly IServiceProvider serviceProvider;

    public TestConsumerFactory(IAddressConfig addressConfig, IServiceProvider serviceProvider)
    {
      this.addressConfig = addressConfig ?? throw new ArgumentNullException(nameof(addressConfig));
      this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IConsumer Create()
    {
      return serviceProvider.GetService(typeof(IConsumer)) as IConsumer;
    }
  }

  public class TestSubscriptionManager : ISubscriptionManager
  {
    public Task Subscribe(string address, string messageName)
    {
      return Task.CompletedTask;
    }

    public Task Unsubscribe(string address, string messageName)
    {
      return Task.CompletedTask;
    }
  }
}