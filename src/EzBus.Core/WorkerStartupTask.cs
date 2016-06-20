using System;
using System.Collections.Generic;
using EzBus.Core.Middleware;
using EzBus.ObjectFactory;

namespace EzBus.Core
{
    public class WorkerStartupTask : IStartupTask
    {
        private readonly IBusConfig busConfig;
        private readonly IObjectFactory objectFactory;
        private List<IMiddleware> middlewares;

        public WorkerStartupTask(IBusConfig busConfig, IObjectFactory objectFactory)
        {
            if (busConfig == null) throw new ArgumentNullException(nameof(busConfig));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            this.busConfig = busConfig;
            this.objectFactory = objectFactory;
        }

        public void Run()
        {
            LoadMiddlewares();

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
            var middlewareInvoker = new MiddlewareInvoker(middlewares);
            middlewareInvoker.Invoke(new MiddlewareContext(channelMessage));
        }

        private void LoadMiddlewares()
        {
            middlewares = new List<IMiddleware>();
            middlewares.AddRange(objectFactory.GetInstances<IPreMiddleware>());
            middlewares.AddRange(objectFactory.GetInstances<IMiddleware>());
            middlewares.AddRange(objectFactory.GetInstances<ISystemMiddleware>());
        }
    }
}