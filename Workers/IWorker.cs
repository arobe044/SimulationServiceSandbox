namespace Sandbox.Workers;

public interface IWorker
{
    Task DoWorkAsync(CancellationToken stoppingToken, int instance);
}
