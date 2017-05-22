using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Core.Middleware;
using EzBus.ObjectFactory;
using EzBus.Utils;

namespace EzBus.Core
{
    public class WorkerStartupTask : IStartupTask
    {
        private readonly IBusConfig busConfig;
        private readonly IObjectFactory objectFactory;

        public WorkerStartupTask(IBusConfig busConfig, IObjectFactory objectFactory)
        {
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
            this.objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
        }

        public void Run()
        {
            for (var i = 0; i < busConfig.WorkerThreads; i++)
            {
                var receivingChannel = objectFactory.GetInstance<IReceivingChannel>();
                receivingChannel.OnMessage = OnMessageReceived;
                var endpointAddress = new EndpointAddress(busConfig.EndpointName);
                var errorEndpointAddress = new EndpointAddress(busConfig.ErrorEndpointName);
                receivingChannel.Initialize(endpointAddress, errorEndpointAddress);
            }
        }

        private void OnMessageReceived(ChannelMessage channelMessage)
        {
            objectFactory.BeginScope();

            var middlewares = LoadMiddlewares();
            var middlewareInvoker = new MiddlewareInvoker(middlewares);
            middlewareInvoker.Invoke(new MiddlewareContext(channelMessage));

            objectFactory.EndScope();
        }

        private IEnumerable<IMiddleware> LoadMiddlewares()
        {
            var middlewares = new List<IMiddleware>();
            var instances = objectFactory.GetInstances<IMiddleware>().ToList();
            middlewares.AddRange(instances.Where(x => x.GetType().ImplementsInterface<IPreMiddleware>()));
            middlewares.AddRange(instances.Where(x => x.GetType().ImplementsInterface<ISystemMiddleware>()));
            middlewares.AddRange(objectFactory.GetInstances<IMiddleware>()
                .Where(x => !x.GetType().ImplementsInterface<IPreMiddleware>()
                            && !x.GetType().ImplementsInterface<ISystemMiddleware>()));
            return middlewares;
        }
    }
}