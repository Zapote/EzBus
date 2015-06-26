namespace EzBus.Samples.Messages
{
    public class Greeting
    {
        public string Message { get; private set; }

        public Greeting(string message)
        {
            Message = message;
        }
    }
}