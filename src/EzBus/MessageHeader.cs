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

        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}