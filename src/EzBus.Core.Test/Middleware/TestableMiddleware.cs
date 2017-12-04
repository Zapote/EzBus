using System;

namespace EzBus.Core.Test.Middleware
{
    public abstract class TestableMiddleware : IMiddleware
    {
        public void Invoke(MiddlewareContext context, Action next)
        {
            IsInvoked = true;

            next();
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }

        public bool IsInvoked { get; set; }
    }
}