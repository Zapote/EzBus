using EzBus.Core.Logging;
using EzBus.Logging;
using NUnit.Framework;

namespace EzBus.Core.Test.Logging
{
    [TestFixture]
    public class TraceLoggerTest
    {
        [Test]
        public void Debug_should_not_be_enabled_when_level_is_info()
        {
            var log = new TraceLogger(LogLevel.Info, "TraceLoggerTest");
            Assert.That(log.IsDebugEnabled, Is.False);
        }

        [Test]
        public void Error_should_be_enabled_when_level_is_info()
        {
            var log = new TraceLogger(LogLevel.Info, "TraceLoggerTest");
            Assert.That(log.IsErrorEnabled, Is.True);
        }

        [Test]
        public void Nothing_should_be_enabled_when_level_is_off()
        {
            var log = new TraceLogger(LogLevel.Off, "TraceLoggerTest");
            Assert.That(log.IsDebugEnabled, Is.False);
            Assert.That(log.IsInfoEnabled, Is.False);
            Assert.That(log.IsWarnEnabled, Is.False);
            Assert.That(log.IsFatalEnabled, Is.False);
            Assert.That(log.IsErrorEnabled, Is.False);
        }
    }
}
