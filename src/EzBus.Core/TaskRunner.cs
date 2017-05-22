using System;
using EzBus.Core.Resolvers;
using EzBus.Logging;
using EzBus.ObjectFactory;
using EzBus.Utils;

namespace EzBus.Core
{
    public class TaskRunner : ITaskRunner
    {
        private static readonly ILogger log = LogManager.GetLogger<TaskRunner>();
        private readonly IObjectFactory objectFactory;

        public TaskRunner(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
        }

        public void RunStartupTasks()
        {
            var startupTasks = objectFactory.GetInstances<IStartupTask>();

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