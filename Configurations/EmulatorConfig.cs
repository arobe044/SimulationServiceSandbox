namespace Sandbox.Configurations;

    public class RabbitMQConfig
    {
        public string HostName { get; set; } = "";
        public int Port { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string RequestExchange { get; set; } = "";
        public string ResponseExchange { get; set; } = "";
        public string TaskExchange { get; set; } = "";
    }

    public class EmulatorConfig
    {
        public int InstanceNum { get; set; }

        ///REQUEST QUEUE
        public string RequestQueue { get; set; } = "";
        public string RequestRoutingKey { get; set; } = "";

        ///RESPONSE QUEUE
        public string ResponseQueue { get; set; } = "";
        public string ResponseRoutingKey { get; set; } = "";

        //TASK QUEUE
        public string TaskQueue { get; set; } = "";
        public string TaskRoutingKey { get; set; } = "";
    }