using System;

namespace EzBus.Core.Test.TestHelpers
{
    public class TestMessage
    {
        public TestMessage(string stringValue)
        {
            StringValue = stringValue;
        }

        public string StringValue { get; set; }
        public TestMessageData TestMessageData { get; set; }
        public TestEnum TestEnum { get; set; }
        public int? NullableIntValue { get; set; }
        public DateTime DateTimeValue { get; set; }
    }
}