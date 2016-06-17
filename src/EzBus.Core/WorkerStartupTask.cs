using System;
using System.Collections.Generic;
using EzBus.Core.Middleware;
using EzBus.ObjectFactory;

namespace EzBus.Core
{
    public class WorkerStartupTask : IStartupTask
    {
        private readonly IHostConfig hostConfig;
        private readonly IObjectFactory objectFactory;
        private List<IMiddleware> middlewares;

        public WorkerStartupTask(IHostConfig hostConfig, IObjectFactory objectFactory)
        {
            if (hostConfig == null) throw new ArgumentNullException(nameof(hostConfig));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            this.hostConfig = hostConfig;
            this.objectFactory = objectFactory;
        }

        public void Run()
        {
            LoadMiddlewares();

            for (var i = 0; i < hostConfig.WorkerThreads; i++)
            {
                var receivingChannel = objectFactory.GetInstance<IReceivingChannel>();
                receivingChannel.OnMessage = OnMessageReceived;
                var endpointAddress = new EndpointAddress(hostConfig.EndpointName);
                var errorEndpointAddress = new EndpointAddress(hostConfig.ErrorEndpointName);
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