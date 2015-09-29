using EzBus.Core.Utils;
using NUnit.Framework;

namespace EzBus.Core.Test.Utils
{
    [TestFixture]
    public class TypeExtensionsTest
    {
        [Test]
        public void GetAssemblyName_returns_name_of_the_assembly()
        {
            Assert.That(this.GetAssemblyName(), Is.EqualTo("EzBus.Core.Test"));
        }

        [Test]
        public void IsLocal_returns_true_when_type_from_EzBus_Core()
        {
            Assert.That(GetType().IsLocal(), Is.False);
            Assert.That(typeof(CoreBus).IsLocal(), Is.True);
        }
    }
}
