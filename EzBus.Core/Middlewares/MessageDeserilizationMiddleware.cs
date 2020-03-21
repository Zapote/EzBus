using System;
using System.Linq;
using EzBus.Serializers;
using Microsoft.Extensions.Logging;

namespace EzBus.Core.Middlewares
{
    internal class MessageDeserilizationMiddleware : IPreMiddleware
    {
        private readonly ILogger<MessageDeserilizationMiddleware> logger;
        private readonly IBodySerializer bodySerializer;
        private readonly IHandlerCache handlerCache;

        public MessageDeserilizationMiddleware(IBodySerializer bodySerializer, IHandlerCache handlerCache, ILogger<MessageDeserilizationMiddleware> logger)
        {
            this.bodySerializer = bodySerializer ?? throw new ArgumentNullException(nameof(bodySerializer));
            this.handlerCache = handlerCache ?? throw new ArgumentNullException(nameof(handlerCache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Invoke(MiddlewareContext context, Action next)
        {
            var channelMessage = context.BasicMessage;
            var messageTypeName = channelMessage.GetHeader(MessageHeaders.MessageFullname);
            var handlerInfos = handlerCache.GetHandlerInfo(messageTypeName).ToList();

            if (handlerInfos.Any())
            {
                var messageType = handlerInfos.First().MessageType;
                context.Message = bodySerializer.Deserialize(channelMessage.BodyStream, messageType);
            }
            else
            {
                context.Message = bodySerializer.Deserialize(channelMessage.BodyStream, typeof(object));
            }

            next();
        }

        public void OnError(Exception ex)
        {
        }
    }
}