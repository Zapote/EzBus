using System;

namespace EzBus
{
    public interface IMiddleware
    {
        void Invoke(object message, Action next);
        void OnError(Exception ex);
    }
}