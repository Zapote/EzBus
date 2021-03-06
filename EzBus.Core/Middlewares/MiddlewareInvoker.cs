﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBus.Core.Middlewares
{
    public class MiddlewareInvoker
    {
        private readonly Queue<IMiddleware> queue;
        private readonly IList<IMiddleware> middlewares;

        public IEnumerable<IMiddleware> Middlewares => middlewares;

        public MiddlewareInvoker(IEnumerable<IMiddleware> middlewares)
        {
            this.middlewares = middlewares.ToList();
            queue = new Queue<IMiddleware>(this.middlewares);
        }

        public async Task Invoke(MiddlewareContext context)
        {
            if (queue.Count == 0) return;
            var mw = queue.Dequeue();
            try
            {
                await mw.Invoke(context, () => Invoke(context));
            }
            catch (Exception ex)
            {
                foreach (var middleware in middlewares)
                {
                    await middleware.OnError(ex);
                }
            }
        }
    }
}
