using EzBus.Logging;
using System;

namespace EzBus.Core
{
    public class BusStarter
    {
        private readonly ITaskRunner taskRunner;
        private static readonly ILogger log = LogManager.GetLogger<BusStarter>();

        public BusStarter(ITaskRunner taskRunner)
        {
            this.taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public void Start()
        {
            log.Verbose("Starting EzBus");
            taskRunner.RunStartupTasks();
        }
    }
}
