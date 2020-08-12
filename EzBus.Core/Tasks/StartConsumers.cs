using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EzBus.Core.Middlewares;
using EzBus.Utils;
using Microsoft.Extensions.Logging;

namespace EzBus.Core
{
    public class StartConsumers : ISystemStartupTask
    {
        private readonly IConsumerFactory consumerFactory;
        private readonly IHandlerInvoker invoker;
        private readonly IWorkerThreadsConfig config;
        private readonly ILogger<StartConsumers> logger;
        private readonly List<IConsumer> consumers = new List<IConsumer>();
        private readonly List<Thread> workerThreads = new List<Thread>();

        public StartConsumers(IConsumerFactory consumerFactory, IHandlerInvoker invoker, IWorkerThreadsConfig config, ILogger<StartConsumers> logger)
        {
            this.consumerFactory = consumerFactory ?? throw new ArgumentNullException(nameof(consumerFactory));
            this.invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Name => "StartConsumers";

        public int Prio => 100;

        public Task Run()
        {
            logger.LogInformation($"{config.WorkerThreads} worker threads configured.");

            for (var i = 0; i < config.WorkerThreads; i++)
            {
               
                var thread = new Thread(StartConsumerThread);
                thread.Start();
                workerThreads.Add(thread);
            }

            return Task.CompletedTask;
        }

        private void StartConsumerThread()
        {
            var consumer = consumerFactory.Create();
            consumer.Consume(OnMessageReceived);
            consumers.Add(consumer);
            logger.LogInformation($"Starting consumer {consumers.Count}");
        }

        private async Task OnMessageReceived(BasicMessage channelMessage)
        {
            await invoker.Invoke(channelMessage);
        }
    }
}