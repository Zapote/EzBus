namespace EzBus
{
    public class MessageHeader
    {
        public MessageHeader()
        {

        }

        public MessageHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }

        public void ChangeValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}