namespace Sandbox.Workers;

public interface IWorker
{
    Task StartAsync(CancellationToken stoppingToken, int instance);
}
