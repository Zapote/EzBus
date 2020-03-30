using System;
using System.Threading.Tasks;
using EzBus;
using Microsoft.Extensions.Logging;

namespace DiceRoller.Service
{
    public class MyMiddleware : IMiddleware
    {
        private readonly ILogger<MyMiddleware> logger;

        public MyMiddleware(ILogger<MyMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task Invoke(MiddlewareContext context, Func<Task> next)
        {
            logger.LogInformation("Before message handled.");
            await next();
            logger.LogInformation("After  message handled.");
        }

        public Task OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}