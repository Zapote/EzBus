using System.Threading.Tasks;
using EzBus;
using Microsoft.Extensions.Logging;

namespace DiceRoller.Worker
{
    public class RunAtStartUp : IStartupTask
    {
        private readonly ILogger<RunAtStartUp> logger;

        public RunAtStartUp(ILogger<RunAtStartUp> logger)
        {
            this.logger = logger;
        }

        public string Name => "RunAtStartUp task";

        public Task Run()
        {
            logger.LogDebug("I am running at startup!");
            return Task.CompletedTask;
        }
    }
}