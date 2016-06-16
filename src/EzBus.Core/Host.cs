using EzBus.Logging;
using System;
using System.Collections.Generic;
using EzBus.Core.Middleware;
using EzBus.ObjectFactory;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class Host
    {
        private static readonly ILogger log = LogManager.GetLogger<Host>();
        private readonly IHostConfig config;
        private readonly IObjectFactory objectFactory;

        public Host(IHostConfig config, IObjectFactory objectFactory)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            this.config = config;
            this.objectFactory = objectFactory;
        }

        public void Start()
        {
            log.Verbose("Starting EzBus Host");

            var taskRunner = objectFactory.GetInstance<ITaskRunner>();
            taskRunner.RunStartupTasks();

            //put in startuptasks?
            CreateListeningWorkers();
        }

        private void OnMessageReceived(ChannelMessage channelMessage)
        {
            var middlewares = new List<IMiddleware>();
            middlewares.AddRange(objectFactory.GetInstances<IPreMiddleware>());
            middlewares.AddRange(objectFactory.GetInstances<IMiddleware>());
            middlewares.AddRange(objectFactory.GetInstances<ISystemMiddleware>());

            var middlewareInvoker = new MiddlewareInvoker(middlewares);
            middlewareInvoker.Invoke(new MiddlewareContext(channelMessage));

            //Deserialize message
            //Invoke customer mw:s
            //Call handler
        }

        private void CreateListeningWorkers()
        {
            for (var i = 0; i < config.WorkerThreads; i++)
            {
                var receivingChannel = objectFactory.GetInstance<IReceivingChannel>();
                receivingChannel.OnMessage = OnMessageReceived;
                var endpointAddress = new EndpointAddress(config.EndpointName);
                var errorEndpointAddress = new EndpointAddress(config.ErrorEndpointName);
                receivingChannel.Initialize(endpointAddress, errorEndpointAddress);
            }
        }
    }
}
