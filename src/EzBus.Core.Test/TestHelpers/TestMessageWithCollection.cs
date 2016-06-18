using System.Collections.Generic;

namespace EzBus.Core.Test.TestHelpers
{
    public class TestMessageWithCollection : TestMessage
    {
        private IList<TestMessageData> dataCollection;

        public TestMessageWithCollection(string stringValue)
            : base(stringValue)
        {
            dataCollection = new List<TestMessageData>();
        }

        public IEnumerable<TestMessageData> DataCollection
        {
            get { return dataCollection; }
            set { dataCollection = new List<TestMessageData>(value); }
        }

        public int? NullableIntValue { get; set; }

        public void AddData(TestMessageData data)
        {
            dataCollection.Add(data);
        }
    }
}