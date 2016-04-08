using System;
using System.Diagnostics;
using EzBus.Logging;

namespace EzBus.Samples.Msmq.Service.Fwk
{
    public class MetricsMiddleware : IMiddleware
    {
        private ILogger logger = LogManager.GetLogger<MetricsMiddleware>();

        public void Invoke(object message, Action next)
        {
            var sw = new Stopwatch();
            sw.Start();
            next();
            sw.Stop();
            logger.DebugFormat("Message {0} handled in {1} ms", message.GetType(), sw.ElapsedMilliseconds);
        }

        public void OnError(Exception ex)
        {

        }
    }
}