using System;
using EzBus.Core.Resolvers;
using EzBus.Core.Utils;
using EzBus.Logging;
using EzBus.ObjectFactory;

namespace EzBus.Core
{
    public class TaskRunner : ITaskRunner
    {
        private static readonly ILogger log = LogManager.GetLogger<TaskRunner>();
        private readonly IObjectFactory objectFactory;

        public TaskRunner(IObjectFactory objectFactory)
        {
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            this.objectFactory = objectFactory;
        }

        public void RunStartupTasks()
        {
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