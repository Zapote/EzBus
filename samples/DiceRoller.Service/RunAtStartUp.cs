using System;
using EzBus;
using EzBus.Logging;

namespace DiceRoller.Service
{
    public class RunAtStartUp : IStartupTask
    {
        private static readonly ILogger log = LogManager.GetLogger<RunAtStartUp>();

        public RunAtStartUp(IDependency dep)
        {

        }

        public string Name => "RunAtStartUp task";
        public void Run()
        {
            log.Info("I am running at startup!");
        }
    }
}