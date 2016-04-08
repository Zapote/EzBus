using System;

namespace EzBus.Core.Test.TestHelpers
{
    public class MiddlewareForTest : IMiddleware
    {
        public void Invoke(object message, Action next)
        {

        }

        public void OnError(Exception ex)
        {

        }
    }
}