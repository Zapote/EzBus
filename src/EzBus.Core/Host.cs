using EzBus.Logging;
using System;

namespace EzBus.Core
{
    public class Host
    {
        private readonly ITaskRunner taskRunner;
        private static readonly ILogger log = LogManager.GetLogger<Host>();

        public Host(ITaskRunner taskRunner)
        {
            if (taskRunner == null) throw new ArgumentNullException(nameof(taskRunner));
            this.taskRunner = taskRunner;
        }

        public void Start()
        {
            log.Verbose("Starting EzBus Host");
            taskRunner.RunStartupTasks();
        }
    }
}
