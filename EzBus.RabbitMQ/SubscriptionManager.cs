using System;
using System.Threading.Tasks;
using EzBus.Utils;
using Microsoft.Extensions.Logging;

namespace EzBus.RabbitMQ
{
  internal class SubscriptionManager(IChannelFactory channelFactory, IAddressConfig addressConf, ILogger<SubscriptionManager> logger) : ISubscriptionManager
  {
    private readonly IAddressConfig addressConf = addressConf ?? throw new ArgumentNullException(nameof(addressConf));
    private readonly ILogger<SubscriptionManager> logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IChannelFactory channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));

    public async Task Subscribe(string address, string messageName)
    {
      try
      {
        var channel = await channelFactory.GetChannel();
        var queue = addressConf.Address.ToLower();
        var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

        logger.LogInformation($"Subscribing to endpoint '{address}'. Routingkey '{routingKey}'");

        await channel.QueueBindAsync(queue, address, routingKey);
      }
      catch (Exception ex)
      {
        logger.LogError($"Failed to subscribe to endpoint {address}", ex);
      }
    }

    public async Task Unsubscribe(string endpoint, string messageName)
    {
      try
      {
        endpoint = endpoint.ToLower();

        var channel = await channelFactory.GetChannel();
        var queue = addressConf.Address.ToLower();
        var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

        logger.LogInformation($"Unsubscribing from endpoint '{endpoint}'. Routingkey '{routingKey}'");

        await channel.QueueUnbindAsync(queue, endpoint, routingKey);
      }
      catch (Exception ex)
      {
        logger.LogError($"Failed to unsubscribe to endpoint {endpoint}", ex);
      }
    }
  }
}
