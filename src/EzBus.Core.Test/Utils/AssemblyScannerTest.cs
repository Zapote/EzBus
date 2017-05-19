using System.Linq;
using EzBus.Core.Test.TestHelpers;
using EzBus.Core.Utils;
using NUnit.Framework;

namespace EzBus.Core.Test.Utils
{
    [TestFixture]
    public class AssemblyScannerTest
    {
        private readonly AssemblyScanner scanner = new AssemblyScanner();

        [Test]
        public void Scanner_should_find_all_handlers_types()
        {
            var handlerTypes = scanner.FindTypes(typeof(IHandle<>));

            Assert.That(handlerTypes.Count(), Is.GreaterThanOrEqualTo(2));
            Assert.That(handlerTypes.Select(x => x.FullName), Contains.Item(typeof(BarHandler).FullName));
            Assert.That(handlerTypes.Select(x => x.FullName), Contains.Item(typeof(FooHandler).FullName));
        }

        [Test]
        public void Scanner_should_find_all_IReceivingChannel_types()
        {
            var handlers = scanner.FindTypes(typeof(IReceivingChannel));

            Assert.That(handlers.Length, Is.GreaterThanOrEqualTo(1));
        }
    }
}
