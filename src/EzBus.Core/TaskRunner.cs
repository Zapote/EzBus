using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Logging;
using EzBus.ObjectFactory;

namespace EzBus.Core
{
    public class TaskRunner : ITaskRunner
    {
        private static readonly ILogger log = LogManager.GetLogger<TaskRunner>();
        private readonly IObjectFactory objectFactory;
        private IEnumerable<IStartupTask> startupTasks;

        public TaskRunner(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
        }

        public void RunStartupTasks()
        {
            startupTasks = objectFactory.GetInstances<IStartupTask>();

            foreach (var task in startupTasks.OrderBy(x => x.Name))
            {
                try
                {
                    log.Info($"Running StartupTask {task.Name}");
                    task.Run();
                }
                catch (Exception ex)
                {
                    log.Warn($"Failed to run StartupTask: {task.Name}", ex);
                }
            }
        }
    }
}