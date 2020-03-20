using EzBus.Core.Middlewares;
using EzBus.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBus.Core
{
    public class HandlerInvoker : IHandlerInvoker
    {
        private readonly IServiceScopeFactory scopeFactory;

        public HandlerInvoker(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        public Task Invoke(BasicMessage basicMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var middlewares = LoadMiddlewares(scope);
                var middlewareInvoker = new MiddlewareInvoker(middlewares);
                middlewareInvoker.Invoke(new MiddlewareContext(basicMessage));
            }

            return Task.CompletedTask;
        }

        private IEnumerable<IMiddleware> LoadMiddlewares(IServiceScope scope)
        {
            var middlewares = new List<IMiddleware>();
            var instances = scope.ServiceProvider.GetServices<IMiddleware>();
            middlewares.AddRange(instances.Where(IsPreMiddleware));
            middlewares.AddRange(instances.Where(IsMiddleware));
            middlewares.AddRange(instances.Where(IsSystemMiddleware));
            return middlewares;
        }

        private static bool IsSystemMiddleware(IMiddleware x)
        {
            return x.GetType().ImplementsInterface<ISystemMiddleware>();
        }

        private static bool IsPreMiddleware(IMiddleware x)
        {
            return x.GetType().ImplementsInterface<IPreMiddleware>();
        }

        private static bool IsMiddleware(IMiddleware x)
        {
            return !x.GetType().ImplementsInterface<IPreMiddleware>() && !x.GetType().ImplementsInterface<ISystemMiddleware>();
        }
    }
}