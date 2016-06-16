using System.Linq;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class HandlerCacheTest
    {
        private readonly HandlerCache handlerCache = new HandlerCache();

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            handlerCache.Prime();
        }

        [Test]
        public void Handler_info_is_returned_when_using_only_class_name()
        {
            var handlerInfo = handlerCache.GetHandlerInfo("MockMessage").Single();

            Assert.That(handlerInfo.HandlerType, Is.EqualTo(typeof(BarHandler)));
            Assert.That(handlerInfo.MessageType, Is.EqualTo(typeof(MockMessage)));
        }

        [Test]
        public void Handler_info_is_returned_when_using_full_name()
        {
            var handlerInfo = handlerCache.GetHandlerInfo("EzBus.Core.Test.TestHelpers.MockMessage").Single();

            Assert.That(handlerInfo.HandlerType, Is.EqualTo(typeof(BarHandler)));
            Assert.That(handlerInfo.MessageType, Is.EqualTo(typeof(MockMessage)));
        }

        [Test]
        public void Null_is_returned_when_no_handlerinfo_exists()
        {
            var handlerInfo = handlerCache.GetHandlerInfo("FaultyMessage").SingleOrDefault();

            Assert.That(handlerInfo, Is.Null);
        }
    }
}
