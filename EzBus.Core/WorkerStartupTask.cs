using System;
using System.Collections.Generic;
using System.Linq;
using EzBus.Core.Middleware;
using EzBus.Logging;
using EzBus.Utils;

namespace EzBus.Core
{
    //public class WorkerStartupTask : IStartupTask
    //{
    //    private static readonly ILogger logger = LogManager.GetLogger<WorkerStartupTask>();
    //    private readonly IHandlerCache handlerCache;
    //    private readonly IConfig busConfig;
    //    private readonly IObjectFactory objectFactory;
    //    private readonly IList<IReceivingChannel> receivingChannels = new List<IReceivingChannel>();

    //    public WorkerStartupTask(IConfig busConfig, IObjectFactory objectFactory, IHandlerCache handlerCache)
    //    {
    //        this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
    //        this.objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
    //        this.handlerCache = handlerCache ?? throw new ArgumentNullException(nameof(handlerCache));
    //    }

    //    public string Name => "WorkerStartUp";

    //    public void Run()
    //    {
    //        if (!handlerCache.HasCustomHandlers())
    //        {
    //            logger.Info("No handlers found - This is a sending endpoint only.");
    //            return;
    //        }

    //        for (var i = 0; i < busConfig.WorkerThreads; i++)
    //        {
    //            var rc = objectFactory.GetInstance<IReceivingChannel>();
    //            var endpointAddress = new EndpointAddress(busConfig.EndpointName);
    //            var errorEndpointAddress = new EndpointAddress(busConfig.ErrorEndpointName);

    //            rc.OnMessage = OnMessageReceived;
    //            rc.Initialize(endpointAddress, errorEndpointAddress);

    //            receivingChannels.Add(rc);
    //        }
    //    }

    //    private void OnMessageReceived(BasicMessage channelMessage)
    //    {
    //        objectFactory.BeginScope();

    //        var middlewares = LoadMiddlewares();
    //        var middlewareInvoker = new MiddlewareInvoker(middlewares);
    //        middlewareInvoker.Invoke(new MiddlewareContext(channelMessage));

    //        objectFactory.EndScope();
    //    }

    //    private IEnumerable<IMiddleware> LoadMiddlewares()
    //    {
    //        var middlewares = new List<IMiddleware>();
    //        var instances = objectFactory.GetInstances<IMiddleware>().ToList();
    //        middlewares.AddRange(instances.Where(IsPreMiddleware));
    //        middlewares.AddRange(instances.Where(IsMiddleware));
    //        middlewares.AddRange(instances.Where(IsSystemMiddleware));
    //        return middlewares;
    //    }

    //    private static bool IsSystemMiddleware(IMiddleware x)
    //    {
    //        return x.GetType().ImplementsInterface<ISystemMiddleware>();
    //    }

    //    private static bool IsPreMiddleware(IMiddleware x)
    //    {
    //        return x.GetType().ImplementsInterface<IPreMiddleware>();
    //    }

    //    private static bool IsMiddleware(IMiddleware x)
    //    {
    //        return !x.GetType().ImplementsInterface<IPreMiddleware>() && !x.GetType().ImplementsInterface<ISystemMiddleware>();
    //    }
    //}
}