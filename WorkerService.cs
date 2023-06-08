using Microsoft.Extensions.Hosting;
using Sandbox.Configurations;
using Sandbox.Factories;
using Microsoft.Extensions.Logging;
using Sandbox.Workers;

namespace Sandbox;

public class WorkerService : BackgroundService
{
    private readonly IWorkerFactory _workerFactory;
    private readonly ILogger<WorkerService> _logger;
    private readonly IDictionary<string, WorkerConfig> _workerDictionary;
    public WorkerService(IWorkerFactory workerFactory, ILogger<WorkerService> logger, IDictionary<string, WorkerConfig> workerDictionary)
    {
        _workerFactory = workerFactory;
        _logger = logger;
        _workerDictionary = workerDictionary;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker service is starting.");
        var workerTypes = _workerDictionary;
        var workers = new List<IWorker>();
        try
        {
            foreach (var workerType in workerTypes)
            {
                for (int i = 0; i < workerType.Value.InstanceNum; i++)
                {
                    var worker = await _workerFactory.CreateWorkerAsync(workerType.Key);
                    workers.Add(worker);
                    _logger.LogInformation($"Starting worker '{workerType.Key}' number {i+1}.");
                    await worker.DoWorkAsync(stoppingToken, i+1);
                }
            }
            _logger.LogInformation("All workers have stopped.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running the worker service.");
        }
        finally
        {
            foreach (var worker in workers)
            {
                if (worker is IDisposable disposableWorker)
                {
                    disposableWorker.Dispose();
                }
            }
            _logger.LogInformation("Worker service has stopped.");
        }
    }
}
