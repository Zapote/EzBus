using System;
using EzBus;
using EzBus.Logging;

namespace DiceRoller.Service
{
    public class MyMiddleware : IMiddleware
    {
        private readonly ILogger log = LogManager.GetLogger<MyMiddleware>();
        private readonly IDependency dependency;

        public MyMiddleware(IDependency dependency)
        {
            this.dependency = dependency;
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            log.Info("Before " + dependency.Id);
            next();
            log.Info("After " + dependency.Id);
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}