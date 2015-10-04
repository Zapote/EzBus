using System;
using EzBus.Core.Resolvers;
using EzBus.Logging;

namespace EzBus.Core
{
    public class TaskRunner
    {
        private static readonly ILogger log = LogManager.GetLogger<TaskRunner>();

        public static void RunStartupTasks(IHostConfig config)
        {
            var startupTasks = StartupTaskResolver.GetStartupTasks();
            foreach (var task in startupTasks)
            {
                var taskName = task.GetType().Name;

                try
                {
                    log.Verbose($"Running task {taskName}");
                    task.Run(config);
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to run startup task: {taskName}", ex);
                }
            }
        }
    }
}