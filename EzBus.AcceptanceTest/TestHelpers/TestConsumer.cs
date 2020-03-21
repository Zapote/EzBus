using EzBus.Core.Serializers;
using System;
using System.Threading.Tasks;

namespace EzBus.AcceptanceTest.TestHelpers
{
  public class TestConsumer : IConsumer
  {
    private readonly IAddressConfig config;
    Action<BasicMessage> onMessage;

    public TestConsumer(IAddressConfig config)
    {
      this.config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public Task Consume(Action<BasicMessage> onMessage)
    {
      this.onMessage = onMessage;
      return Task.CompletedTask;
    }

    public void Invoke(BasicMessage basicMessage)
    {
      var dest = basicMessage.GetHeader(MessageHeaders.Destination);
      if (dest != config.Address) return;
      onMessage.Invoke(basicMessage);
    }
  }

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
}