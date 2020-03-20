using System;
using System.Threading.Tasks;

namespace EzBus.Core.Tasks
{
    public class LoadHandlers :ISystemStartupTask
    {
        private readonly IHandlerCache handlerCache;

        public LoadHandlers(IHandlerCache handlerCache)
        {
            this.handlerCache = handlerCache ?? throw new ArgumentNullException(nameof(handlerCache));
        }

        public int Prio => 300;

        public string Name => "LoadHandlers";

        public Task Run()
        {
            handlerCache.Prime();
            return Task.CompletedTask;
        }
    }
}
