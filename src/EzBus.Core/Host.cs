using EzBus.Core.Resolvers;
using EzBus.Core.Serilizers;
using EzBus.Logging;
using System;
using System.Linq;
using EzBus.Core.Utils;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class Host
    {
        private static readonly ILogger log = HostLogManager.GetLogger(typeof(Host));
        private readonly IObjectFactory objectFactory;
        private readonly ISendingChannel sendingChannel;
        private readonly IMessageSerializer messageSerializer = new XmlMessageSerializer();
        private readonly HandlerCache handlerCache = new HandlerCache();
        private readonly string endpointName;
        private readonly string endpointErrorName;
        private readonly int workerThreads;
        private readonly int numberOfRetrys;

        public Host(HostConfig hostConfig)
        {
            if (hostConfig == null) throw new ArgumentNullException(nameof(hostConfig));

            workerThreads = hostConfig.WorkerThreads;
            numberOfRetrys = hostConfig.NumberOfRetrys;
            endpointName = hostConfig.EndpointName;
            endpointErrorName = $"{endpointName}.error";

            objectFactory = ObjectFactoryResolver.GetObjectFactory();
            sendingChannel = ChannelResolver.GetSendingChannel();

            handlerCache.Prime();
        }

        public void Start()
        {
            if (handlerCache.NumberOfEntries == 0) return;

            log.Verbose("Starting Ezbus Host");

            objectFactory.Initialize();

            var subscriptionStorage = SubscriptionStorageResolver.GetSubscriptionStorage();
            subscriptionStorage.Initialize(endpointName);

            for (var i = 0; i < workerThreads; i++)
            {
                var receivingChannel = ChannelResolver.GetReceivingChannel();
                receivingChannel.OnMessageReceived += OnMessageReceived;
                receivingChannel.Initialize(new EndpointAddress(endpointName), new EndpointAddress(endpointErrorName));
            }

            Subscribe();
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var messageTypeName = e.Message.Headers.ElementAt(0).Value;
            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName);

            object message = null;

            foreach (var info in handlerInfo)
            {
                var handlerType = info.HandlerType;
                var messageType = info.MessageType;

                if (message == null)
                {
                    message = messageSerializer.Deserialize(e.Message.BodyStream, messageType);
                }

                log.VerboseFormat("Invoking handler {0}", handlerType.Name);

                var result = InvokeHandler(handlerType, message);

                if (!result.Success)
                {
                    PlaceMessageOnErrorQueue(e.Message, result.Exception.InnerException);
                }
            }
        }

        private InvokationResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;

            for (var i = 0; i < numberOfRetrys; i++)
            {
                var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });
                var messageFilters = new IMessageFilter[0];

                objectFactory.BeginScope();

                try
                {
                    var handler = objectFactory.CreateInstance(handlerType);
                    messageFilters = MessageFilterResolver.GetMessageFilters(objectFactory);

                    var isLocalMessage = message.GetType().IsLocal();
                    if (!isLocalMessage) messageFilters.Apply(x => x.Before());
                    methodInfo.Invoke(handler, new[] { message });
                    if (!isLocalMessage) messageFilters.Apply(x => x.After());

                    break;
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Attempt {1}: Failed to handle message '{0}'.", message.GetType().Name, i + 1), ex.InnerException);
                    success = false;
                    messageFilters.Apply(x => x.OnError(ex.InnerException));
                    exception = ex.InnerException;
                }

                objectFactory.EndScope();
            }

            return new InvokationResult(success, exception);
        }

        private void Subscribe()
        {
            var subscriptionManager = SubscriptionManagerResolver.GetSubscriptionManager();
            subscriptionManager.Subscribe(endpointName);
        }

        private void PlaceMessageOnErrorQueue(ChannelMessage message, Exception exception)
        {
            var level = 0;

            while (exception != null)
            {
                var headerName = $"ErrorMessage L{level}";
                var value = $"{DateTime.Now}: {exception.Message}";
                message.AddHeader(headerName, value);
                exception = exception.InnerException;
                level++;
            }

            sendingChannel.Send(new EndpointAddress(endpointErrorName), message);
        }
    }
}
