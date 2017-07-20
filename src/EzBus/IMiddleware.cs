using System;

namespace EzBus
{
    public interface IMiddleware
    {
        void Invoke(MiddlewareContext context, Action next);
        void OnError(Exception ex);
    }
}