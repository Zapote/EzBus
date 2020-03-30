using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EzBus;
using Microsoft.Extensions.Logging;

namespace DiceRoller.Service
{
    public class RollTheDiceHandler : IHandle<RollTheDice>
    {
        private readonly ILogger<RollTheDiceHandler> logger;
        private readonly IPublisher publisher;

        public RollTheDiceHandler(ILogger<RollTheDiceHandler> logger, IPublisher publisher)
        {
            this.logger = logger;
            this.publisher = publisher;
        }

        public async Task Handle(RollTheDice message)
        {
            logger.LogDebug($"Rolling the dice {message.Attempts} times");

            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < message.Attempts; i++)
            {
                var result = new Random().Next(1, 7);
                await publisher.Publish(new DiceRolled { Result = result });
                logger.LogInformation($"Result published {message.Order}");
            }
            sw.Stop();

            logger.LogDebug($"{message.Attempts} times took ${sw.Elapsed.TotalSeconds} s");
        }
    }
}
