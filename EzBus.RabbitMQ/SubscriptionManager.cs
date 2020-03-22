using System;
using System.Threading.Tasks;
using EzBus.Utils;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EzBus.RabbitMQ
{
  internal class SubscriptionManager : ISubscriptionManager
  {
    private readonly IAddressConfig addressConf;
    private readonly ILogger<SubscriptionManager> logger;
    private readonly IChannelFactory channelFactory;

    public SubscriptionManager(IChannelFactory channelFactory, IAddressConfig addressConf, ILogger<SubscriptionManager> logger)
    {
      this.channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
      this.addressConf = addressConf ?? throw new ArgumentNullException(nameof(addressConf));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Subscribe(string address, string messageName)
    {
      try
      {
        var channel = channelFactory.GetChannel();
        var queue = addressConf.Address.ToLower();
        var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

        logger.LogInformation($"Subscribing to endpoint '{address}'. Routingkey '{routingKey}'");

        channel.QueueBind(queue, address, routingKey);
      }
      catch (Exception ex)
      {
        logger.LogError($"Failed to subscribe to endpoint {address}", ex);
      }

      return Task.CompletedTask;
    }

    public Task Unsubscribe(string endpoint, string messageName)
    {
      try
      {
        endpoint = endpoint.ToLower();

        var channel = channelFactory.GetChannel();
        var queue = addressConf.Address.ToLower();
        var routingKey = messageName.IsNullOrEmpty() ? "#" : messageName;

        logger.LogInformation($"Unsubscribing from endpoint '{endpoint}'. Routingkey '{routingKey}'");

        channel.QueueUnbind(queue, endpoint, routingKey);
      }
      catch (Exception ex)
      {
        logger.LogError($"Failed to unsubscribe to endpoint {endpoint}", ex);
      }

      return Task.CompletedTask;
    }
  }
}
