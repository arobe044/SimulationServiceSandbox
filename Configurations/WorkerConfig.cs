namespace Sandbox.Configurations;

    public class RabbitMQConfig
    {
        public string HostName { get; set; } = "";
        public int Port { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string RequestExchangeName { get; set; } = "";
        public string ResponseExchangeName { get; set; } = "";
        public string TaskExchangeName { get; set; } = "";
    }

    public class WorkerConfig
    {
        public int InstanceNum { get; set; }

        ///REQUEST QUEUE
        public string RequestQueueName { get; set; } = "";
        public string RequestRoutingKey { get; set; } = "";

        ///RESPONSE QUEUE
        public string ResponseQueueName { get; set; } = "";
        public string ResponseRoutingKey { get; set; } = "";
    }