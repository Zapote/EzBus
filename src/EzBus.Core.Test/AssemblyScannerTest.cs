using System.Linq;
using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    public class AssemblyScannerTest
    {
        private AssemblyScanner scanner = new AssemblyScanner();

        public void TestSetup()
        {

        }

        public void Scanner_should_find_all_handlers_types()
        {
            var handlers = scanner.FindTypeInAssemblies(typeof(IMessageHandler<>));

            Assert.That(handlers.Count(), Is.EqualTo(2));
            Assert.That(handlers, Contains.Item(typeof(BarHandler)));
            Assert.That(handlers, Contains.Item(typeof(FooHandler)));
        }
    }
}
