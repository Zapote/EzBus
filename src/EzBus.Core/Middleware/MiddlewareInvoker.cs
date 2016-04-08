using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBus.Core.Middleware
{
    public class MiddlewareInvoker
    {
        private readonly IMiddleware[] middlewares;

        public MiddlewareInvoker(IEnumerable<IMiddleware> middlewares)
        {
            this.middlewares = middlewares.ToArray();
        }

        public void Invoke(object message, Action next)
        {
            InvokeNext(message, next, 0);
        }

        private void InvokeNext(object message, Action next, int index)
        {
            if (index == middlewares.Length)
            {
                next();
                return;
            }

            InvokeNext(message, () => middlewares[index].Invoke(message, next), index + 1);
        }
    }
}
