using System;
using System.Threading.Tasks;

namespace EzBus.Core.Test.Middleware
{
    public abstract class TestableMiddleware : IMiddleware
    {
        public Task Invoke(MiddlewareContext context, Func<Task> next)
        {
            IsInvoked = true;
            return next();
        }

        public Task OnError(Exception ex)
        {
            throw new NotImplementedException();
        }

        public bool IsInvoked { get; set; }
    }
}