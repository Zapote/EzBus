namespace EzBus.Samples.Messages
{
    public class SayHello
    {
        public SayHello(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return string.Format("Hello {0}", Name);
        }
    }
}
