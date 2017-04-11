using System;
using System.Diagnostics;
using log4net;

namespace EzBus.Samples.Msmq.Service.Middlewares
{
    public class MetricsMiddleware : IMiddleware
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MetricsMiddleware));

        public void Invoke(MiddlewareContext context, Action next)
        {
            var sw = new Stopwatch();
            sw.Start();
            next();
            sw.Stop();
            log.Info($"Message {context.Message.GetType()} handled in {sw.ElapsedMilliseconds} ms");
        }

        public void OnError(Exception ex)
        {

        }
    }
}