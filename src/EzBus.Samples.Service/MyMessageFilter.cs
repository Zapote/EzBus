using System;

namespace EzBus.Samples.Service
{
    public class MyMessageFilter : IMessageFilter
    {
        private readonly IOtherDependency dependency;

        public MyMessageFilter(IOtherDependency dependency)
        {
            if (dependency == null) throw new ArgumentNullException("dependency");
            this.dependency = dependency;
        }

        public void Before()
        {
        }

        public void After(Exception ex)
        {
        }
    }
}