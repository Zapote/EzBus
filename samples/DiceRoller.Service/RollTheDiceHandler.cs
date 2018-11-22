using System;
using System.Diagnostics;
using EzBus;
using EzBus.Logging;

namespace DiceRoller.Service
{
    public class RollTheDiceHandler : IHandle<RollTheDice>
    {
        private static readonly ILogger log = LogManager.GetLogger<RollTheDiceHandler>();
        private readonly IDependency dependency;

        public RollTheDiceHandler(IDependency dependency)
        {
            this.dependency = dependency;
        }

        public void Handle(RollTheDice message)
        {
            log.Debug($"DependencyId {dependency.Id}");
            log.Debug($"Rolling the dice {message.Attempts} times");
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < message.Attempts; i++)
            {
                var result = new Random().Next(1, 7);
                Bus.Publish(new DiceRolled { Result = result });
            }
            sw.Stop();
            log.Debug($"{message.Attempts} times took ${sw.Elapsed.TotalSeconds} s");
        }
    }
}