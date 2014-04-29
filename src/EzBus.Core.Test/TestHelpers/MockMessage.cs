namespace EzBus.Core.Test.TestHelpers
{
    public class MockMessage
    {
        public MockMessage(string stringValue)
        {
            StringValue = stringValue;
            MockData = new MockData();
            MockEnum = MockEnum.Bar;
        }

        public string StringValue { get; protected set; }
        public MockData MockData { get; protected set; }
        public MockEnum MockEnum { get; protected set; }
    }

    public class FailingMessage
    {

    }
}