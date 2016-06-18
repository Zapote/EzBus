namespace EzBus.Core.Test.TestHelpers
{
    public class TestMessage
    {
        public TestMessage(string stringValue)
        {
            StringValue = stringValue;
            TestMessageData = new TestMessageData();
            TestEnum = TestEnum.Bar;
        }

        public string StringValue { get; protected set; }
        public TestMessageData TestMessageData { get; protected set; }
        public TestEnum TestEnum { get; protected set; }
    }
}