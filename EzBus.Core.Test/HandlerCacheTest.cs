using System;
using System.Linq;
using Xunit;

namespace EzBus.Core.Test
{
    public class HandlerCacheTest
    {
        [Fact]
        public void Handler_info_is_returned_when_using_only_message_name()
        {
            var handlerCache = new HandlerCache();
            handlerCache.Prime();

            const string messageTypeName = "TestMessage";

            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName).FirstOrDefault();

            Assert.Equal("BarHandler", handlerInfo.HandlerType.Name);
            Assert.Equal(messageTypeName, handlerInfo.MessageType.Name);
        }

        [Fact]
        public void Handler_info_is_returned_when_using_full_name()
        {
            var handlerCache = new HandlerCache();
            handlerCache.Prime();
            const string messageTypeName = "EzBus.Core.Test.TestHelpers.TestMessage";

            var result = handlerCache.GetHandlerInfo(messageTypeName).FirstOrDefault();

            Assert.Equal("BarHandler", result.HandlerType.Name);
            Assert.Equal(messageTypeName, result.MessageType.FullName);
        }

        [Fact]
        public void Null_is_returned_when_no_handlerinfo_exists()
        {
            var handlerCache = new HandlerCache();
            handlerCache.Prime();
            var handlerInfo = handlerCache.GetHandlerInfo("FaultyMessage").SingleOrDefault();

            Assert.Null(handlerInfo);
        }

        [Fact]
        public void Null_is_returned_on_similar_message_names()
        {
            var handlerCache = new HandlerCache();
            handlerCache.Prime();
            const string messageTypeName = "Message";

            var result = handlerCache.GetHandlerInfo(messageTypeName);

            Assert.Equal(0, result.Count());
        }
    }
}
