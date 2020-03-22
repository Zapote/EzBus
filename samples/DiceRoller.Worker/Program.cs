using EzBus;
using EzBus.Core;
using EzBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiceRoller.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    var bus = BusFactory
                       .Address("diceroller-worker")
                       .UseRabbitMQ()
                       .AddServices(services)
                       .CreateBus();

                    services.AddSingleton(bus);
                });
    }
}

