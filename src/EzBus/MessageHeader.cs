namespace EzBus
{
    public class MessageHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value);
        }
    }
}