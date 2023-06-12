using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sandbox.Configurations;
using Sandbox.Controllers;
using Sandbox.Emulators;

namespace Sandbox.Factories;

public class EmulatorFactory : IEmulatorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQConfig _rmqConfig;
    private readonly IRabbitMQConsumer _rmqConsumer;
    private readonly IRabbitMQProducer _rmqProducer;

    public EmulatorFactory(IServiceProvider serviceProvider, RabbitMQConfig rmqConfig, IRabbitMQConsumer rmqConsumer, IRabbitMQProducer rmqProducer)
    {
        _serviceProvider = serviceProvider;
        _rmqConfig = rmqConfig;
        _rmqConsumer = rmqConsumer;
        _rmqProducer = rmqProducer;
    }
    public async Task<IEmulator> CreateEmulatorAsync(string emulatorType)
    {
        switch (emulatorType)
        {
            case "LocusEmulator":
                return await CreateLocusEmulatorAsync();
            case "EmulatorA":
                return await CreateEmulatorAAsync();
            case "EmulatorB":
                return await CreateEmulatorBAsync();
            default:
                throw new ArgumentException($"Unknown emulator type '{emulatorType}'.", nameof(emulatorType));
        }
    }
    private async Task<IEmulator> CreateLocusEmulatorAsync()
    {
        //var messageSender = _serviceProvider.GetRequiredService<IMessageSender>();
        var logger = _serviceProvider.GetRequiredService<ILogger<LocusEmulator>>();
        return new LocusEmulator(logger, _rmqProducer, _rmqConsumer);
    }
    
    private async Task<IEmulator> CreateEmulatorAAsync()
    {
        //var messageSender = _serviceProvider.GetRequiredService<IMessageSender>();
        var logger = _serviceProvider.GetRequiredService<ILogger<EmulatorA>>();
        return new EmulatorA(logger, _rmqConfig);
    }

    private async Task<IEmulator> CreateEmulatorBAsync()
    {
        //var messageSender = _serviceProvider.GetRequiredService<IMessageSender>();
        var logger = _serviceProvider.GetRequiredService<ILogger<EmulatorB>>();
        return new EmulatorB(logger, _rmqConfig);
    }
}
