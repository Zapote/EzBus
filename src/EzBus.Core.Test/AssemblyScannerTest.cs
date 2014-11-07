using System.Linq;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class AssemblyScannerTest
    {
        private readonly AssemblyScanner scanner = new AssemblyScanner();

        [SetUp]
        public void TestSetup()
        {

        }

        [Test]
        public void Scanner_should_find_all_handlers_types()
        {
            var handlers = scanner.FindTypes(typeof(IHandle<>));

            Assert.That(handlers.Count(), Is.GreaterThanOrEqualTo(2));
            Assert.That(handlers, Contains.Item(typeof(BarHandler)));
            Assert.That(handlers, Contains.Item(typeof(FooHandler)));
        }

        [Test]
        public void Scanner_should_find_all_IReceivingChannel_types()
        {
            var handlers = scanner.FindTypes(typeof(IReceivingChannel));

            Assert.That(handlers.Count(), Is.GreaterThanOrEqualTo(1));
            Assert.That(handlers, Contains.Item(typeof(InMemoryMessageChannel)));
        }
    }
}
