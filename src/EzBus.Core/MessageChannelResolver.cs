using System;
using System.Linq;

namespace EzBus.Core
{
    public class MessageChannelResolver
    {
        private static Type sendingChannelType;
        private static Type receivingChannelType;

        static MessageChannelResolver()
        {
            ResolveTypes();
        }

        public static ISendingChannel GetSendingChannel()
        {
            return (ISendingChannel)sendingChannelType.CreateInstance();
        }

        public static IReceivingChannel GetReceivingChannel()
        {
            return (IReceivingChannel)receivingChannelType.CreateInstance();
        }

        private static void ResolveTypes()
        {
            var assemblyScanner = new AssemblyScanner();
            var sendingChannels = assemblyScanner.FindTypes<ISendingChannel>();
            var localns = typeof(MessageChannelResolver).Namespace;

            foreach (var type in sendingChannels.Where(type => type.Namespace != localns))
            {
                sendingChannelType = type;
                break;
            }

            if (sendingChannelType == null)
            {
                //TODO: add default (inmemory?) channel
                //channelTypes.SendingChannelType = new 
            }

            var receivingChannels = assemblyScanner.FindTypes<IReceivingChannel>();

            foreach (var type in receivingChannels.Where(type => type.Namespace != localns))
            {
                receivingChannelType = type;
                break;
            }

            if (sendingChannelType == null)
            {
                //TODO: add default (inmemory?) channel
                //channelTypes.SendingChannelType = new 
            }
        }
    }
}