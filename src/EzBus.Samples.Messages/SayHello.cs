namespace EzBus.Samples.Messages
{
    public class SayHello
    {
        public SayHello(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
