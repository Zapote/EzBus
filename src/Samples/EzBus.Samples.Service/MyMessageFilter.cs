using System;

namespace EzBus.Samples.Service
{
    public class MyMessageFilter : IMessageFilter
    {
        public MyMessageFilter(IOtherDependency dependency)
        {
            if (dependency == null) throw new ArgumentNullException("dependency");
        }

        public void Before()
        {
        }

        public void After()
        {
        }

        public void OnError(Exception ex)
        {
        }
    }
}