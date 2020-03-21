using System;
using EzBus;
using Microsoft.Extensions.Logging;

namespace DiceRoller.Service
{
    public class MyMiddleware : IMiddleware
    {
        private readonly ILogger<MyMiddleware> logger;

        public MyMiddleware(ILogger<MyMiddleware> logger )
        {
            this.logger = logger;
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            logger.LogInformation("Before message handled.");
            next();
            logger.LogInformation("After  message handled.");
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}