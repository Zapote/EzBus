using EzBus.Core.Resolvers;
using EzBus.Logging;
using Xunit;

namespace EzBus.Core.Test.Resolvers
{
    public class LoggerFactoryResolverTest
    {
        [Fact]
        public void LoggerFactory_should_be_ConsoleLoggerFactory()
        {
            var loggerFactory = LoggerFactoryResolver.GetLoggerFactory();

            Assert.Equal(typeof(ConsoleLoggerFactory), loggerFactory.GetType());
        }
    }
}