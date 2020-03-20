using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task Run<T>() where T : ITask
        {
            IEnumerable<T> tasks;

            using var scope = serviceScopeFactory.CreateScope();
            {
                tasks = scope.ServiceProvider.GetServices<T>().OrderBy(x => x.Name).ToArray();
            }

            if (typeof(T) == typeof(ISystemStartupTask))
            {
                tasks = tasks.Cast<ISystemStartupTask>().OrderByDescending(x => x.Prio).Cast<T>();
            }

            foreach (var task in tasks)
            {
                try
                {
                    log.Info($"Running task {task.Name}");
                    await task.Run();
                }
                catch (Exception ex)
                {
                    log.Warn($"Failed to run task: {task.Name}", ex);
                }
            }
        }
    }
}