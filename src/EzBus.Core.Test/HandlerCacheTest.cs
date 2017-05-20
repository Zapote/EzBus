using EzBus.Core.Test.TestHelpers;
using System.Linq;
using EzBus.Core;
using Xunit;

namespace EzBus.Core.Test
{
    public class HandlerCacheTest
    {
        private readonly HandlerCache handlerCache = new HandlerCache();


        public HandlerCacheTest()
        {
            handlerCache.Prime();
        }

        [Fact]
        public void Handler_info_is_returned_when_using_only_class_name()
        {
            const string messageTypeName = "TestMessage";

            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName).Single();

            Assert.Equal("BarHandler", handlerInfo.HandlerType.Name);
            Assert.Equal(messageTypeName, handlerInfo.MessageType.Name);
        }

        [Fact]
        public void Handler_info_is_returned_when_using_full_name()
        {
            const string messageTypeName = "EzBus.Core.Test.TestHelpers.TestMessage";

            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName).Single();

            Assert.Equal("BarHandler", handlerInfo.HandlerType.Name);
            Assert.Equal(messageTypeName, handlerInfo.MessageType.FullName);
        }

        [Fact]
        public void Null_is_returned_when_no_handlerinfo_exists()
        {
            var handlerInfo = handlerCache.GetHandlerInfo("FaultyMessage").SingleOrDefault();

            Assert.Null(handlerInfo);
        }
    }
}
