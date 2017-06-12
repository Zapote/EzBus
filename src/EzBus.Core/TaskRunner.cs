using System;
using System.Collections.Generic;
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

            foreach (var task in startupTasks)
            {
                var taskName = task.GetType().Name;
                try
                {
                    log.Info($"Running StartupTask {taskName}");
                    task.Run();
                }
                catch (Exception ex)
                {
                    log.Warn($"Failed to run StartupTask: {taskName}", ex);
                }
            }
        }
    }
}