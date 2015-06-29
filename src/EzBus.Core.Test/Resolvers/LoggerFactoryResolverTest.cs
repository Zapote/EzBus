using EzBus.Core.Logging;
using EzBus.Core.Resolvers;
using NUnit.Framework;

namespace EzBus.Core.Test.Resolvers
{
    [TestFixture]
    public class LoggerFactoryResolverTest
    {
        [Test]
        public void Receiving_channel_should_be_FakeMessageChannel()
        {
            var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();
            Assert.That(loggerFactory, Is.InstanceOf<TraceLoggerFactory>());
        }
    }
}