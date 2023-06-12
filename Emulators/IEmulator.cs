namespace Sandbox.Emulators;

public interface IEmulator
{
    Task StartAsync(CancellationToken stoppingToken, int instance);
}
