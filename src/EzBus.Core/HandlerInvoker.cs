using System;
using System.Linq;
using EzBus.Core.Middleware;
using EzBus.Core.Utils;
using EzBus.Logging;
using EzBus.ObjectFactory;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class HandlerInvoker
    {
        private static readonly ILogger log = LogManager.GetLogger<HandlerInvoker>();
        private readonly IObjectFactory objectFactory;
        private readonly IHandlerCache handlerCache;
        private readonly IMessageSerializer messageSerializer;

        public HandlerInvoker(IObjectFactory objectFactory)
        {
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            this.objectFactory = objectFactory;

            handlerCache = objectFactory.GetInstance<IHandlerCache>();
            handlerCache.Prime();
            messageSerializer = objectFactory.GetInstance<IMessageSerializer>();
        }

        public void Invoke(ChannelMessage channelMessage)
        {
            var messageTypeName = channelMessage.Headers.ElementAt(0).Value;
            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName);

            object message = null;

            //foreach (var info in handlerInfo)
            //{
            //    var handlerType = info.HandlerType;
            //    var messageType = info.MessageType;

            //    if (message == null)
            //    {
            //        message = messageSerializer.Deserialize(channelMessage.BodyStream, messageType);
            //    }

            //    log.VerboseFormat("Invoking handler {0}", handlerType.Name);

            //    var result = InvokeHandler(handlerType, message);

            //    if (!result.Success)
            //    {
            //        PlaceMessageOnErrorQueue(channelMessage, result.Exception.InnerException);
            //    }
            //}
        }

        private InvokationResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;
            var numberOfRetrys = 5;// config.NumberOfRetrys;

            for (var i = 0; i < numberOfRetrys; i++)
            {
                var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });
                var middlewares = new IMiddleware[0];

                objectFactory.BeginScope();

                try
                {
                    var handler = objectFactory.GetInstance(handlerType);
                    middlewares = objectFactory.GetInstances<IMiddleware>().ToArray();

                    var isLocalMessage = message.GetType().IsLocal();

                    Action next = () => methodInfo.Invoke(handler, new[] { message });

                    if (isLocalMessage)
                    {
                        next();
                        break;
                    }

                    //new MiddlewareInvoker(middlewares).Invoke(message, next);
                    break;
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Attempt {1}: Failed to handle message '{0}'.", message.GetType().Name, i + 1), ex.InnerException);
                    success = false;
                    middlewares.Apply(x => x.OnError(ex.InnerException));
                    exception = ex.InnerException;
                }

                objectFactory.EndScope();
            }

            return new InvokationResult(success, exception);
        }

        private void PlaceMessageOnErrorQueue(ChannelMessage message, Exception exception)
        {
            //var level = 0;

            //while (exception != null)
            //{
            //    var headerName = $"EzBus.ErrorMessage L{level}";
            //    var value = $"{DateTime.Now}: {exception.Message}";
            //    message.AddHeader(headerName, value);
            //    exception = exception.InnerException;
            //    level++;
            //}

            //var endpointAddress = new EndpointAddress(config.ErrorEndpointName);
            //sendingChannel.Send(endpointAddress, message);
        }

        private void PrimeHandlerCache()
        {
            handlerCache.Prime();
        }
    }
}
