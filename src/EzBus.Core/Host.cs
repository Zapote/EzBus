using EzBus.Logging;
using System;
using EzBus.ObjectFactory;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class Host
    {
        private readonly ITaskRunner taskRunner;
        private static readonly ILogger log = LogManager.GetLogger<Host>();
        private readonly IObjectFactory objectFactory;

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
