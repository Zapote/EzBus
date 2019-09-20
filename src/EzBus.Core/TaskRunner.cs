using System;
using System.Linq;
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
            this.objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
        }

        public void RunStartupTasks()
        {
            objectFactory.BeginScope();

            var tasks = objectFactory.GetInstances<IStartupTask>();

            foreach (var task in tasks.OrderBy(x => x.Name))
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

            objectFactory.EndScope();
        }

        public void RunShutdownTasks()
        {
            objectFactory.BeginScope();

            var tasks = objectFactory.GetInstances<IShutdownTask>();

            foreach (var task in tasks.OrderBy(x => x.Name))
            {
                try
                {
                    log.Info($"Running ShutdownTask {task.Name}");
                    task.Run();
                }
                catch (Exception ex)
                {
                    log.Warn($"Failed to run ShutdownTask: {task.Name}", ex);
                }
            }

            objectFactory.EndScope();
        }
    }
}