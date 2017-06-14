using EzBus.Core.Logging;
using EzBus.Logging;
using Xunit;

namespace EzBus.Core.Test.Logging
{
    public class ConsoleLoggerTest
    {
        [Fact]
        public void Debug_should_not_be_enabled_when_level_is_info()
        {
            var log = new ConsoleLogger(LogLevel.Info, "TraceLoggerTest");
            Assert.False(log.IsDebugEnabled);
        }

        [Fact]
        public void Error_should_be_enabled_when_level_is_info()
        {
            var log = new ConsoleLogger(LogLevel.Info, "TraceLoggerTest");
            Assert.True(log.IsErrorEnabled);
        }

        [Fact]
        public void Nothing_should_be_enabled_when_level_is_off()
        {
            var log = new ConsoleLogger(LogLevel.Off, "TraceLoggerTest");
            Assert.False(log.IsDebugEnabled);
            Assert.False(log.IsInfoEnabled);
            Assert.False(log.IsWarnEnabled);
            Assert.False(log.IsFatalEnabled);
            Assert.False(log.IsErrorEnabled);
        }
    }
}
