using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EzBus.Core.Middlewares;
using EzBus.Utils;

namespace EzBus.Core
{
    public class StartConsumers : ISystemStartupTask
    {
        private readonly IConsumerFactory consumerFactory;
        private readonly IHandlerInvoker invoker;
        private readonly IBusConfig config;

        public StartConsumers(IConsumerFactory consumerFactory, IHandlerInvoker invoker, IBusConfig config)
        {
            this.consumerFactory = consumerFactory ?? throw new ArgumentNullException(nameof(consumerFactory));
            this.invoker = invoker;
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string Name => "StartConsumers";

        public int Prio => 100;

        public async Task Run()
        {
            for (var i = 0; i < config.WorkerThreads; i++)
            {
                var consumer = consumerFactory.Create();
                await consumer.Consume(OnMessageReceived);
            }
        }

        private void OnMessageReceived(BasicMessage channelMessage)
        {
            invoker.Invoke(channelMessage);
        }
    }
}