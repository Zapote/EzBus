using System;

namespace EzBus.Core.Test.TestHelpers
{
    public class TestMessage
    {
        public string StringValue { get; set; } = "";
        public TestMessageData TestMessageData { get; set; } = new();
        public TestEnum TestEnum { get; set; }
        public int? NullableIntValue { get; set; }
        public DateTime DateTimeValue { get; set; }
    }
}