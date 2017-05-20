using EzBus.Core.Logging;
using EzBus.Core.Resolvers;
using Xunit;

namespace EzBus.Core.Test.Resolvers
{
    public class LoggerFactoryResolverTest
    {
        [Fact]
        public void LoggerFactory_should_be_TraceLoggerFactory()
        {
            var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();

            Assert.Equal(typeof(TraceLoggerFactory), loggerFactory.GetType());
        }
    }
}