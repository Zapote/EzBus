using System;

namespace EzBus.Msmq
{
    public static class EndpointAddressExtensions
    {
        private const string directPrefix = @"FormatName:DIRECT=OS:";

        public static string GetQueueName(this EndpointAddress address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            var machineName = address.MachineName;

            if (string.IsNullOrEmpty(address.MachineName))
            {
                machineName = Environment.MachineName;
            }

            return $@"{machineName}\private$\{address.QueueName}";
        }

        public static string GetQueuePath(this EndpointAddress address)
        {
            return $"{directPrefix}{GetQueueName(address)}";
        }
    }
}

