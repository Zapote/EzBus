using System.Configuration;
using EzBus.Core.Config;
using NUnit.Framework;

namespace EzBus.Core.Test.Config
{
    [TestFixture]
    public class DestinationSectionConfigTest
    {
        private readonly DestinationCollection destinations = DestinationSection.Section.Destinations;

        [Test]
        public void First_section_is_generic_for_assembly()
        {
            var destination = destinations[0];

            Assert.That(destination.Endpoint, Is.EqualTo("acme.input"));
            Assert.That(destination.Assembly, Is.EqualTo("Acme.Messages"));
            Assert.That(destination.Message, Is.EqualTo(""));

        }

        [Test]
        public void First_section_is_specific_for_message()
        {
            var destination = destinations[1];

            Assert.That(destination.Endpoint, Is.EqualTo("acme.input.fastlane"));
            Assert.That(destination.Assembly, Is.EqualTo("Acme.Messages"));
            Assert.That(destination.Message, Is.EqualTo("Acme.Messages.DoAction"));
        }
    }
}
