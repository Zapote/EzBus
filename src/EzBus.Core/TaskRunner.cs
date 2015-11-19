using System;
using EzBus.Core.Resolvers;
using EzBus.Core.Utils;
using EzBus.Logging;

namespace EzBus.Core
{
    public class TaskRunner
    {
        private static readonly ILogger log = LogManager.GetLogger<TaskRunner>();

        public static void RunStartupTasks()
        {
            var objectFactory = ObjectFactoryResolver.Get();
            var startupTasks = new AssemblyScanner().FindTypes<IStartupTask>();
            foreach (var taskType in startupTasks)
            {
                var taskName = taskType.Name;

                try
                {
                    log.Info($"Running StartupTask {taskName}");
                    var task = (IStartupTask)objectFactory.GetInstance(taskType);
                    task.Run();
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to run StartupTask: {taskName}", ex);
                }
            }
        }
    }
}