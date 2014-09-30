using EzBus.Core.Serilizers;
using EzBus.Logging;
using EzBus.Serilizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EzBus.Core
{
    public class Host
    {
        private static readonly ILogger log = HostLogManager.GetLogger(typeof(Host));

        private ISendingChannel sendingChannel;
        private readonly IObjectFactory objectFactory;
        private readonly IMessageSerilizer messageSerializer;
        private readonly ISubscriptionStorage subscriptionStorage;
        private HandlerCache handlerCache;
        private string inputQueue;
        private string errorQueue;
        private readonly int workerThreads;
        private readonly int numberOfRetrys;

        public Host(HostConfig hostConfig)
        {
            if (hostConfig == null) throw new ArgumentNullException("hostConfig");
            objectFactory = hostConfig.ObjectFactory;
            workerThreads = hostConfig.WorkerThreads;
            numberOfRetrys = hostConfig.NumberOfRetrys;
            subscriptionStorage = hostConfig.SubscriptionStorage;
            messageSerializer = new XmlMessageSerializer();
        }

        public void Start()
        {
            log.Debug("Starting");

            var scanner = new AssemblyScanner();
            var handlerTypes = scanner.FindTypes(typeof(IHandle<>));

            if (NoCustomHandlersFound(handlerTypes))
            {
                log.Debug("No custom handlers found.");
                return;
            }

            objectFactory.Initialize();
            objectFactory.Register<ISubscriptionStorage>(subscriptionStorage.GetType());

            handlerCache = new HandlerCache();

            foreach (var handlerType in handlerTypes)
            {
                handlerCache.Add(handlerType);
            }

            inputQueue = CreateEndpointName();
            log.DebugFormat("Endpoint: {0}", inputQueue);
            errorQueue = string.Format("{0}.error", inputQueue);

            sendingChannel = MessageChannelResolver.GetSendingChannel();
            log.DebugFormat("SendingChannel: {0}", sendingChannel.GetType());
            for (var i = 0; i < workerThreads; i++)
            {
                var receivingChannel = MessageChannelResolver.GetReceivingChannel();
                receivingChannel.Initialize(new EndpointAddress(inputQueue), new EndpointAddress(errorQueue));
                receivingChannel.OnMessageReceived += OnMessageReceived;
            }

            SendSubscriptionMessages();

            log.Debug("Started");
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

                log.DebugFormat("Invoking handler: {0}", handlerType.Name);

                var result = InvokeHandler(handlerType, message);

                if (!result.Success)
                {
                    PlaceMessageOnErrorQueue(e.Message, result.Exception.InnerException);
                }
            }
        }

        private void SendSubscriptionMessages()
        {
            var subscriptions = Config.SubscriptionSection.Section.Subscriptions;

            foreach (Config.SubscriptionElement subscription in subscriptions)
            {
                var subscriptionMessage = new SubscriptionMessage
                {
                    Endpoint = string.Format("{0}@{1}", inputQueue, Environment.MachineName)
                };

                var destination = EndpointAddress.Parse(subscription.Endpoint);

                log.DebugFormat("Subscribing to: {0}", destination);
                sendingChannel.Send(destination, ChannelMessageFactory.CreateChannelMessage(subscriptionMessage, messageSerializer));
            }
        }

        private static bool NoCustomHandlersFound(IList<Type> handlerTypes)
        {
            if (handlerTypes.Count == 0)
            {
                return true;
            }
            return handlerTypes.Count == 1 && handlerTypes[0] == typeof(SubscriptionMessageHandler);
        }

        private void PlaceMessageOnErrorQueue(ChannelMessage message, Exception exception)
        {
            var level = 0;

            while (exception != null)
            {
                var headerName = string.Format("ErrorMessage L{0}", level);
                var value = string.Format("{0}: {1}", DateTime.Now, exception.Message);
                message.AddHeader(headerName, value);
                exception = exception.InnerException;
                level++;
            }

            sendingChannel.Send(new EndpointAddress(errorQueue), message);
        }

        private InvokationResult InvokeHandler(Type handlerType, object message)
        {
            var success = true;
            Exception exception = null;
            var methodInfo = handlerType.GetMethod("Handle", new[] { message.GetType() });

            objectFactory.BeginScope();

            var handler = objectFactory.CreateInstance(handlerType);
            var messageFilters = MessageFilterResolver.GetMessageFilters(objectFactory);

            for (var i = 0; i < numberOfRetrys; i++)
            {
                try
                {
                    messageFilters.Apply(x => x.Before());
                    methodInfo.Invoke(handler, new[] { message });
                    messageFilters.Apply(x => x.After(null));
                    break;
                }
                catch (Exception ex)
                {
                    log.Error("Failed to handle message!", ex);
                    exception = ex;
                    success = false;
                    messageFilters.Apply(x => x.After(ex));
                }
            }

            objectFactory.EndScope();

            return new InvokationResult(success, exception);
        }

        private string CreateEndpointName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return GetType().Assembly.GetName().Name;
            return entryAssembly.GetName().Name;
        }
    }
}
