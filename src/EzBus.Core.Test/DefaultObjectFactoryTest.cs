using EzBus.Core.Test.TestHelpers;
using NUnit.Framework;

namespace EzBus.Core.Test
{
    [TestFixture]
    public class DefaultObjectFactoryTest
    {
        [Test]
        public void Can_create_object_with_default_constructor()
        {
            var obj = new DefaultObjectFactory().CreateInstance(typeof(BarHandler));

            Assert.That(obj, Is.Not.Null);
        }
    }
}