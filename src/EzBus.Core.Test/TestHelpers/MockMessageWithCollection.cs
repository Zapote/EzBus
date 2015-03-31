using System.Collections.Generic;

namespace EzBus.Core.Test.TestHelpers
{
    public class MockMessageWithCollection : MockMessage
    {
        private IList<MockData> dataCollection;

        public MockMessageWithCollection(string stringValue)
            : base(stringValue)
        {
            dataCollection = new List<MockData>();
        }

        public IEnumerable<MockData> DataCollection
        {
            get { return dataCollection; }
            set { dataCollection = new List<MockData>(value); }
        }

        public int? NullableIntValue { get; set; }

        public void AddData(MockData data)
        {
            dataCollection.Add(data);
        }
    }
}