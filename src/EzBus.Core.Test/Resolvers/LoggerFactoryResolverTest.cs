using EzBus.Core.Logging;
using EzBus.Core.Resolvers;
using NUnit.Framework;

namespace EzBus.Core.Test.Resolvers
{
    [TestFixture]
    public class LoggerFactoryResolverTest
    {
        [Test]
        public void LoggerFactory_should_be_TraceLoggerFactory()
        {
            var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();

            Assert.That(loggerFactory.GetType().FullName, Is.EqualTo(typeof(TraceLoggerFactory).FullName));
        }
    }
}