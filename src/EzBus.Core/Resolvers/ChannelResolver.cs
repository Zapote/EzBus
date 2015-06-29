using System;
using System.Linq;

namespace EzBus.Core.Resolvers
{
    internal class ChannelResolver
    {
        private static Type sendingChannelType;
        private static Type receivingChannelType;

        static ChannelResolver()
        {
            ResolveTypes();
        }

        public ISendingChannel GetSendingChannel()
        {
            return (ISendingChannel)sendingChannelType.CreateInstance();
        }

        public IReceivingChannel GetReceivingChannel()
        {
            return (IReceivingChannel)receivingChannelType.CreateInstance();
        }

        private static void ResolveTypes()
        {
            var assemblyScanner = new AssemblyScanner();

            var sendingChannels = assemblyScanner.FindTypes<ISendingChannel>();
            sendingChannelType = sendingChannels.LastOrDefault(type => !type.IsLocal()) ??
                                 typeof(InMemoryMessageChannel);


            var receivingChannels = assemblyScanner.FindTypes<IReceivingChannel>();
            receivingChannelType = receivingChannels.LastOrDefault(t => !t.IsLocal()) ??
                                   typeof(InMemoryMessageChannel);
        }
    }
}