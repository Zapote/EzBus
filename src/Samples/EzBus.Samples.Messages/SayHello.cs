namespace EzBus.Samples.Messages
{
    public class SayHello
    {
        public SayHello(string name, int? age = null)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public int? Age { get; private set; }

        public override string ToString()
        {
            return string.Format("Hello {0}", Name);
        }
    }
}
