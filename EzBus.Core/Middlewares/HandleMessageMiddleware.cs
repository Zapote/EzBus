﻿using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EzBus.Core.Middlewares
{
    internal class HandleMessageMiddleware : ISystemMiddleware
    {
        private readonly IHandlerCache handlerCache;
        private readonly IBusConfig busConfig;
        private readonly ILogger<HandleMessageMiddleware> logger;
        private readonly IServiceProvider serviceProvider;

        public HandleMessageMiddleware(IHandlerCache handlerCache, IServiceProvider serviceProvider, IBusConfig busConfig, ILogger<HandleMessageMiddleware> logger)
        {
            this.handlerCache = handlerCache ?? throw new ArgumentNullException(nameof(handlerCache));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.busConfig = busConfig ?? throw new ArgumentNullException(nameof(busConfig));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(MiddlewareContext context, Func<Task> next)
        {
            var basicMessage = context.BasicMessage;
            var messageTypeName = basicMessage.GetHeader(MessageHeaders.MessageFullname);
            var handlerInfos = handlerCache.GetHandlerInfo(messageTypeName);

            foreach (var info in handlerInfos)
            {
                var handlerType = info.HandlerType;

                logger.LogDebug($"Invoking handler {handlerType.Name}");

                var result = await InvokeHandler(handlerType, context.Message);

                if (!result.Success)
                {
                    throw result.Exception;
                }
            }

            await next();
        }

        public Task OnError(Exception ex)
        {
            return Task.CompletedTask;
        }

        private async Task<InvokationResult> InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;

            for (var i = 0; i < busConfig.NumberOfRetries; i++)
            {
                var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });

                try
                {
                    var handler = serviceProvider.GetService(handlerType);
                    var task = (Task)methodInfo.Invoke(handler, new[] { message });
                    await task.ConfigureAwait(false);
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Attempt {i + 1}: Failed to handle message '{message.GetType().Name}' {ex}", ex);
                    success = false;
                    exception = ex;
                }
            }

            return new InvokationResult(success, exception);
        }
    }
}