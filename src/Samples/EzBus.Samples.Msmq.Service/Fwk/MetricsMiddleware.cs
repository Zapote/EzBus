using System;
using System.Diagnostics;
using EzBus.Logging;

namespace EzBus.Samples.Msmq.Service.Fwk
{
    public class MetricsMiddleware : IMiddleware
    {
        private readonly ILogger logger = LogManager.GetLogger<MetricsMiddleware>();

        public void Invoke(MiddlewareContext context, Action next)
        {
            var sw = new Stopwatch();
            sw.Start();
            next();
            sw.Stop();
            logger.Debug($"Message {context.Message.GetType()} handled in {sw.ElapsedMilliseconds} ms");
        }

        public void OnError(Exception ex)
        {

        }
    }
}