namespace Sandbox.Controllers;

public interface IRabbitMQProducer
{
    void PublishTask(string task);

    void PublishResponse(string response);
}   