using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;
using System.Linq;
using EzBus.Core;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class HandlerCacheTest
    {
        private readonly HandlerCache handlerCache = new HandlerCache();

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            handlerCache.Prime();
        }

        [Test]
        public void Handler_info_is_returned_when_using_only_class_name()
        {
            const string messageTypeName = "TestMessage";

            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName).Single();

            Assert.That(handlerInfo.HandlerType.Name, Is.EqualTo("BarHandler"));
            Assert.That(handlerInfo.MessageType.Name, Is.EqualTo(messageTypeName));
        }

        [Test]
        public void Handler_info_is_returned_when_using_full_name()
        {
            const string messageTypeName = "EzBus.Core.Test.TestHelpers.TestMessage";

            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName).Single();

            Assert.That(handlerInfo.HandlerType.Name, Is.EqualTo("BarHandler"));
            Assert.That(handlerInfo.MessageType.FullName, Is.EqualTo(messageTypeName));
        }

        [Test]
        public void Null_is_returned_when_no_handlerinfo_exists()
        {
            var handlerInfo = handlerCache.GetHandlerInfo("FaultyMessage").SingleOrDefault();

            Assert.That(handlerInfo, Is.Null);
        }
    }
}
