using EzBus.Core.Test.TestHelpers;
using EzBus.Core.Utils;
using Xunit;

namespace EzBus.Core.Test.Utils
{
    public class AssemblyScannerTest
    {
        private readonly AssemblyScanner scanner = new AssemblyScanner();

        [Fact]
        public void Scanner_should_find_all_handlers_types()
        {
            var handlerTypes = scanner.FindTypes(typeof(IHandle<>));
            Assert.Contains(typeof(BarHandler), handlerTypes);
            Assert.Contains(typeof(FooHandler), handlerTypes);
        }

        [Fact]
        public void Scanner_should_find_all_IReceivingChannel_types()
        {
            var channels = scanner.FindTypes(typeof(IReceivingChannel));

            Assert.Equal(channels.Length, 1);
        }
    }
}
