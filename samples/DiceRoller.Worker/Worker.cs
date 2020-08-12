using System;
using System.Threading;
using System.Threading.Tasks;
using EzBus;
using EzBus.Core;
using EzBus.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiceRoller.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private IBus bus;

        public Worker(IBus bus, ILogger<Worker> logger)
        {
            this.logger = logger;
            this.bus = bus;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await bus.Start();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await bus.Stop();
        }
    }
}
