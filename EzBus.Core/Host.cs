using System;
using EzBus.Logging;
using EzBus.Core.Resolvers;

namespace EzBus.Core
{
    public class Host : IHost
    {
        private static readonly ILogger log = LogManager.GetLogger("EzBus");
        private readonly IBusConfig busConfig;
        private readonly ITaskRunner taskRunner;

        public Host(IBusConfig busConfig, ITaskRunner taskRunner)
        {
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
            this.taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));

            ConfigureLogging();
        }

        private void ConfigureLogging()
        {
            var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();
            LogManager.Configure(loggerFactory, busConfig.LogLevel);
        }

        public void Start()
        {
            log.Info("Starting EzBus");
            taskRunner.RunStartupTasks();
            log.Info("EzBus started");
        }

        public void Stop()
        {
            log.Info("Stopping EzBus");
            taskRunner.RunShutdownTasks();
            log.Info("EzBus stopped");
        }
    }
}
