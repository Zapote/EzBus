using EzBus.Utils;
using Xunit;

namespace EzBus.Core.Test.Utils
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void GetAssemblyName_returns_name_of_the_assembly()
        {
            Assert.Equal("EzBus.Core.Test", this.GetAssemblyName());
        }

        [Fact]
        public void IsLocal_returns_true_when_type_from_EzBus_Core()
        {
            Assert.True(typeof(Bus).IsLocal());
        }
    }
}
