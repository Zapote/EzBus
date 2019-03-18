using System;
using System.Linq;
using Xunit;

namespace EzBus.Core.Test
{
    public class HandlerCacheTest
    {
        private static readonly HandlerCache handlerCache = new HandlerCache();

        static HandlerCacheTest()
        {
            handlerCache.Prime();
        }

        [Fact]
        public void Handler_info_is_returned_when_using_only_class_name()
        {
            const string messageTypeName = "TestMessage";

            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName).FirstOrDefault();

            Assert.Equal("BarHandler", handlerInfo.HandlerType.Name);
            Assert.Equal(messageTypeName, handlerInfo.MessageType.Name);
        }

        [Fact]
        public void Handler_info_is_returned_when_using_full_name()
        {
            const string messageTypeName = "EzBus.Core.Test.TestHelpers.TestMessage";

            var result = handlerCache.GetHandlerInfo(messageTypeName).Single();

            Assert.Equal("BarHandler", result.HandlerType.Name);
            Assert.Equal(messageTypeName, result.MessageType.FullName);
        }

        [Fact]
        public void Null_is_returned_when_no_handlerinfo_exists()
        {
            var handlerInfo = handlerCache.GetHandlerInfo("FaultyMessage").SingleOrDefault();

            Assert.Null(handlerInfo);
        }
    }
}
