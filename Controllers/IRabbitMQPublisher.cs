namespace Sandbox.Controllers;

public interface IRabbitMQPublisher
{
    void BuildTaskQueue(string emulatorType);
    void PublishTask(string emulatorType, string task);
    void PublishResponse(string emulatorType, string response);
}   