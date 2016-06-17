using System;
using EzBus.Logging;
using EzBus.ObjectFactory;

namespace EzBus.Core.Middleware
{
    internal class HandleMessageMiddleware : ISystemMiddleware
    {
        private static readonly ILogger log = LogManager.GetLogger<HandleMessageMiddleware>();
        private readonly IHandlerCache handlerCache;
        private readonly IObjectFactory objectFactory;
        private readonly IHostConfig hostConfig;

        public HandleMessageMiddleware(IHandlerCache handlerCache, IObjectFactory objectFactory, IHostConfig hostConfig)
        {
            if (handlerCache == null) throw new ArgumentNullException(nameof(handlerCache));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            if (hostConfig == null) throw new ArgumentNullException(nameof(hostConfig));
            this.handlerCache = handlerCache;
            this.objectFactory = objectFactory;
            this.hostConfig = hostConfig;
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            var channelMessage = context.ChannelMessage;
            var messageTypeName = channelMessage.GetHeader(MessageHeaders.MessageType);
            var handlerInfos = handlerCache.GetHandlerInfo(messageTypeName);

            foreach (var info in handlerInfos)
            {
                var handlerType = info.HandlerType;

                log.Verbose($"Invoking handler {handlerType.Name}");

                var result = InvokeHandler(handlerType, context.Message);

                if (!result.Success)
                {
                    throw result.Exception;
                }
            }

            next();
        }

        public void OnError(Exception ex)
        {

        }

        private InvokationResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;

            for (var i = 0; i < hostConfig.NumberOfRetrys; i++)
            {
                var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });

                objectFactory.BeginScope();

                try
                {
                    var handler = objectFactory.GetInstance(handlerType);
                    methodInfo.Invoke(handler, new[] { message });
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Attempt {1}: Failed to handle message '{0}'.", message.GetType().Name, i + 1), ex.InnerException);
                    success = false;
                    exception = ex.InnerException;
                }

                objectFactory.EndScope();
            }

            return new InvokationResult(success, exception);
        }
    }
}