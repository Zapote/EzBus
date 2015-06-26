using System;
using log4net;

namespace EzBus.Samples.Msmq.Service.MessageFilters
{
    public class MyMessageFilter : IMessageFilter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MyMessageFilter));

        public void Before()
        {
            log.Info("Before message handled");
        }

        public void After()
        {
            log.Info("After message handled");
        }

        public void OnError(Exception ex)
        {
            log.Error("OnError", ex);
        }
    }
}