using System;
using System.Diagnostics;
using EzBus;
using EzBus.Logging;

namespace DiceRoller.Service
{
    public class RollTheDiceHandler : IHandle<RollTheDice>, IHandle<Reset>
    {
        private static readonly ILogger log = LogManager.GetLogger<RollTheDiceHandler>();
        private readonly IPublisher publisher;

        public RollTheDiceHandler(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public void Handle(RollTheDice message)
        {
            log.Debug($"Rolling the dice {message.Attempts} times");
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < message.Attempts; i++)
            {
                var result = new Random().Next(1, 7);
                publisher.Publish(new DiceRolled { Result = result });
            }
            sw.Stop();
            log.Debug($"{message.Attempts} times took ${sw.Elapsed.TotalSeconds} s");
        }

        public void Handle(Reset m)
        {
            throw new NotImplementedException();
        }
    }
}
