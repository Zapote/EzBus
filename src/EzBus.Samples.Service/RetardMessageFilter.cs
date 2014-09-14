using System;

namespace EzBus.Samples.Service
{
    public class RetardMessageFilter : IMessageFilter
    {
        private readonly IDependency dependency;

        public RetardMessageFilter(IDependency dependency)
        {
            if (dependency == null) throw new ArgumentNullException("dependency");
            this.dependency = dependency;
        }

        public void Before()
        {
            Console.WriteLine(dependency.Id);
        }

        public void After(Exception ex)
        {
            Console.WriteLine(dependency.Id);
        }
    }
}