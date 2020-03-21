using System;
using System.Diagnostics;
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

        public void Handle(RollTheDice message)
        {
            logger.LogDebug($"Rolling the dice {message.Attempts} times");
            
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < message.Attempts; i++)
            {
                var result = new Random().Next(1, 7);
                publisher.Publish(new DiceRolled { Result = result });
            }
            sw.Stop();

            logger.LogDebug($"{message.Attempts} times took ${sw.Elapsed.TotalSeconds} s");
        }
    }
}
