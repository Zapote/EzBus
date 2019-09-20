namespace EzBus.Msmq
{
    public class UnsubscribeMessage
    {
        public string Endpoint { get; set; }
        public string MessageName { get; set; }
    }
}