using Microsoft.Extensions.Hosting;
using Sandbox.Configurations;
using Sandbox.Factories;
using Microsoft.Extensions.Logging;
using Sandbox.Emulators;

namespace Sandbox;

public class EmulatorService : BackgroundService
{
    private readonly IEmulatorFactory _emulatorFactory;
    private readonly ILogger<EmulatorService> _logger;
    private readonly IDictionary<string, EmulatorConfig> _emulatorDictionary;
    public EmulatorService(IEmulatorFactory emulatorFactory, ILogger<EmulatorService> logger, IDictionary<string, EmulatorConfig> emulatorDictionary)
    {
        _emulatorFactory = emulatorFactory;
        _logger = logger;
        _emulatorDictionary = emulatorDictionary;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Emulator service is starting.");
        var emulatorTypes = _emulatorDictionary;
        var emulators = new List<IEmulator>();
        try
        {
            foreach (var emulatorType in emulatorTypes)
            {
                for (int i = 0; i < emulatorType.Value.InstanceNum; i++)
                {
                    var emulator = await _emulatorFactory.CreateEmulatorAsync(emulatorType.Key);
                    emulators.Add(emulator);
                    _logger.LogInformation($"Starting emulator '{emulatorType.Key}' number {i+1}.");
                    await emulator.StartAsync(stoppingToken, i+1);
                }
            }
            _logger.LogInformation("All emulators have stopped.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running the emulator service.");
        }
        finally
        {
            foreach (var emulator in emulators)
            {
                if (emulator is IDisposable disposableEmulator)
                {
                    disposableEmulator.Dispose();
                }
            }
            _logger.LogInformation("Emulator service has stopped.");
        }
    }
}
