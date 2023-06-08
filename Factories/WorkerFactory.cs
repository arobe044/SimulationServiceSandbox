using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sandbox.Configurations;
using Sandbox.Controllers;
using Sandbox.Workers;

namespace Sandbox.Factories;

public class WorkerFactory : IWorkerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQConfig _rmqConfig;
    private readonly IRabbitMQConsumer _rmqConsumer;
    private readonly IRabbitMQProducer _rmqProducer;

    public WorkerFactory(IServiceProvider serviceProvider, RabbitMQConfig rmqConfig, IRabbitMQConsumer rmqConsumer, IRabbitMQProducer rmqProducer)
    {
        _serviceProvider = serviceProvider;
        _rmqConfig = rmqConfig;
        _rmqConsumer = rmqConsumer;
        _rmqProducer = rmqProducer;
    }
    public async Task<IWorker> CreateWorkerAsync(string workerType)
    {
        switch (workerType)
        {
            case "LocusEmulator":
                return await CreateLocusEmulatorAsync();
            case "WorkerA":
                return await CreateWorkerAAsync();
            case "WorkerB":
                return await CreateWorkerBAsync();
            default:
                throw new ArgumentException($"Unknown worker type '{workerType}'.", nameof(workerType));
        }
    }
    private async Task<IWorker> CreateLocusEmulatorAsync()
    {
        //var messageSender = _serviceProvider.GetRequiredService<IMessageSender>();
        var logger = _serviceProvider.GetRequiredService<ILogger<LocusEmulator>>();
        return new LocusEmulator(logger, _rmqProducer, _rmqConsumer);
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
