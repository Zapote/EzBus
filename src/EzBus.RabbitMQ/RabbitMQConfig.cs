namespace EzBus.RabbitMQ
{
    public class RabbitMQConfig : IRabbitMQConfig
    {
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public string Uri { get; set; } = "amqp://localhost";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}