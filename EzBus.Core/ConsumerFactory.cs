using System;
using EzBus.Logging;
using EzBus.Core.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace EzBus.Core
{
    public class ConsumerFactory : IConsumerFactory
    {
        private readonly IServiceProvider provider;

        public ConsumerFactory(IServiceProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IConsumer Create()
        {
            try
            {
                return provider.GetRequiredService<IConsumer>();
            }
            catch (Exception)
            {
                throw new Exception("No consumer type registered.");
            }
        }
    }
}
