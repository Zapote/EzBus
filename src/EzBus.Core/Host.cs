using EzBus.Core.Resolvers;
using EzBus.Logging;
using System;
using System.Linq;
using EzBus.Core.Utils;
using EzBus.Serializers;

namespace EzBus.Core
{
    public class Host
    {
        private readonly HostConfig config;
        private static readonly ILogger log = LogManager.GetLogger<Host>();
        private readonly IObjectFactory objectFactory;
        private readonly ISendingChannel sendingChannel;
        private readonly IMessageSerializer messageSerializer;
        private readonly HandlerCache handlerCache = new HandlerCache();
        private readonly string endpointErrorName;

        public Host(HostConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            this.config = config;
            endpointErrorName = $"{config.EndpointName}.error";

            objectFactory = ObjectFactoryResolver.GetObjectFactory();
            sendingChannel = SendingChannelResolver.GetChannel();
            messageSerializer = MessageSerlializerResolver.GetSerializer();
        }

        public void Start()
        {
            if (!LoadHandlers()) return;

            log.Verbose("Starting Ezbus Host");

            objectFactory.Initialize();
            TaskRunner.RunStartupTasks(config);
            CreateListeningWorkers();
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
            var numberOfRetrys = config.NumberOfRetrys;
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

        private void CreateListeningWorkers()
        {
            for (var i = 0; i < config.WorkerThreads; i++)
            {
                var receivingChannel = ReceivingChannelResolver.GetChannel();
                receivingChannel.OnMessageReceived += OnMessageReceived;
                receivingChannel.Initialize(new EndpointAddress(config.EndpointName), new EndpointAddress(endpointErrorName));
            }
        }

        private bool LoadHandlers()
        {
            handlerCache.Prime();

            if (handlerCache.NumberOfEntries > 0) return true;

            log.Warn("No handlers found. Host will not be started.");
            return false;
        }

        private void Subscribe()
        {
            var subscriptionManager = SubscriptionManagerResolver.GetSubscriptionManager();
            subscriptionManager.Subscribe(config.EndpointName);
        }

        private void PlaceMessageOnErrorQueue(ChannelMessage message, Exception exception)
        {
            var level = 0;

            while (exception != null)
            {
                var headerName = $"EzBus.ErrorMessage L{level}";
                var value = $"{DateTime.Now}: {exception.Message}";
                message.AddHeader(headerName, value);
                exception = exception.InnerException;
                level++;
            }

            sendingChannel.Send(new EndpointAddress(endpointErrorName), message);
        }
    }
}
