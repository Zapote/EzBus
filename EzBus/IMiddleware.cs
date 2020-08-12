using System;
using System.Threading.Tasks;

namespace EzBus
{
    public interface IMiddleware
    {
        Task Invoke(MiddlewareContext context, Func<Task> next);
        Task OnError(Exception ex);
    }
}