using Sandbox.Entities;
using RabbitMQ.Client;

namespace Sandbox.Controllers;

public interface IRabbitMQConsumer
{
    void ConsumeRequest(string emulatorType, Action<string> messageHandler);
    void ConsumeTask(Action<string> messageHandler); //should we need to consume automatically and use lambda to process (so that we can keep task on queue til its processed)
    void StopConsuming(string consumerTag);
}   