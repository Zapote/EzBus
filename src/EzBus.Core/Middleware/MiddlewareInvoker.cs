using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBus.Core.Middleware
{
    public class MiddlewareInvoker
    {
        private readonly Queue<IMiddleware> queue;

        public MiddlewareInvoker(IEnumerable<IMiddleware> middlewares)
        {
            queue = new Queue<IMiddleware>(middlewares);
        }

        public void Invoke(MiddlewareContext context)
        {
            if (queue.Count == 0) return;
            var mw = queue.Dequeue();
            mw.Invoke(context, () => Invoke(context));
        }
    }
}
