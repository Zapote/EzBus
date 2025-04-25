using System;
using System.Linq;
using EzBus.Core.Test.TestHelpers;
using EzBus.Core.Utils;
using Xunit;

namespace EzBus.Core.Test
{
    public class HandlerCacheTest
    {
        private HandlerCache handlerCache = new HandlerCache(new NullLogger<HandlerCache>(), new AssemblyScanner(new AssemblyFinder(new NullLogger<AssemblyFinder>()), new NullLogger<AssemblyScanner>()));

        [Fact]
        public void Handler_info_is_returned_when_using_only_message_name()
        {
            handlerCache.Prime();

            const string messageTypeName = "TestMessage";

            var handlerInfo = handlerCache.GetHandlerInfo(messageTypeName).FirstOrDefault();

            Assert.Equal("BarHandler", handlerInfo.HandlerType.Name);
            Assert.Equal(messageTypeName, handlerInfo.MessageType.Name);
        }

        [Fact]
        public void Handler_info_is_returned_when_using_full_name()
        {
            handlerCache.Prime();
            const string messageTypeName = "EzBus.Core.Test.TestHelpers.TestMessage";

            var result = handlerCache.GetHandlerInfo(messageTypeName).FirstOrDefault();

            Assert.Equal("BarHandler", result.HandlerType.Name);
            Assert.Equal(messageTypeName, result.MessageType.FullName);
        }

        [Fact]
        public void Null_is_returned_when_no_handlerinfo_exists()
        {
            handlerCache.Prime();
            var handlerInfo = handlerCache.GetHandlerInfo("FaultyMessage").SingleOrDefault();

            Assert.Null(handlerInfo);
        }

        [Fact]
        public void Null_is_returned_on_similar_message_names()
        {
            handlerCache.Prime();
            const string messageTypeName = "Message";

            var result = handlerCache.GetHandlerInfo(messageTypeName);

            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void NumberOfEntries_shall_be_two()
        {
            handlerCache.Prime();
            Assert.Equal(2, handlerCache.NumberOfEntries);
        }
    }
}
