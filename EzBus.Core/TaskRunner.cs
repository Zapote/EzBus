using System;
using System.Linq;
using EzBus.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace EzBus.Core
{
    public class TaskRunner : ITaskRunner
    {
        private static readonly ILogger log = LogManager.GetLogger<TaskRunner>();
        private readonly IServiceScopeFactory serviceScopeFactory;

        public TaskRunner(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public void RunStartupTasks()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var tasks = scope.ServiceProvider.GetServices<IStartupTask>();

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
            }
        }

        public void RunShutdownTasks()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var tasks = scope.ServiceProvider.GetServices<IShutdownTask>();

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
            }
        }
    }
}