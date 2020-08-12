namespace EzBus
{
    public class MiddlewareContext
    {
        public MiddlewareContext(BasicMessage message)
        {
            BasicMessage = message;
        }

        public BasicMessage BasicMessage { get; private set; }
        public object Message { get; set; }
    }
}