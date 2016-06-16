using System;

namespace EzBus.Core.Test.TestHelpers
{
    public class MiddlewareForTest : IMiddleware
    {
        public void Invoke(MiddlewareContext context, Action next)
        {
            next();
        }

        public void OnError(Exception ex)
        {

        }
    }
}