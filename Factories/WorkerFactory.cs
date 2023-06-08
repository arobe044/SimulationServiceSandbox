using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sandbox.Configurations;
using Sandbox.Workers;

namespace Sandbox.Factories;

public class WorkerFactory : IWorkerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQConfig _rmqConfig;

    public WorkerFactory(IServiceProvider serviceProvider, RabbitMQConfig rmqConfig)
    {
        _serviceProvider = serviceProvider;
        _rmqConfig = rmqConfig;
    }
    public async Task<IWorker> CreateWorkerAsync(string workerType)
    {
        switch (workerType)
        {
            case "WorkerA":
                return await CreateWorkerAAsync();
            case "WorkerB":
                return await CreateWorkerBAsync();
            default:
                throw new ArgumentException($"Unknown worker type '{workerType}'.", nameof(workerType));
        }
    }
    private async Task<IWorker> CreateWorkerAAsync()
    {
        //var messageSender = _serviceProvider.GetRequiredService<IMessageSender>();
        var logger = _serviceProvider.GetRequiredService<ILogger<WorkerA>>();
        return new WorkerA(logger, _rmqConfig);
    }
    private async Task<IWorker> CreateWorkerBAsync()
    {
        //var messageSender = _serviceProvider.GetRequiredService<IMessageSender>();
        var logger = _serviceProvider.GetRequiredService<ILogger<WorkerB>>();
        return new WorkerB(logger, _rmqConfig);
    }
}
