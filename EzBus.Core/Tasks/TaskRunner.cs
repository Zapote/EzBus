using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EzBus.Core
{
    public class TaskRunner : ITaskRunner
    {
        private readonly ILogger<TaskRunner> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public TaskRunner(ILogger<TaskRunner> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                    logger.LogInformation($"Running task: '{task.Name}'");
                    await task.Run();
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Failed to run task: '{task.Name}'", ex);
                }
            }
        }
    }
}