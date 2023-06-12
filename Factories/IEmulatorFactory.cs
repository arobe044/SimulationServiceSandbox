using Sandbox.Emulators;

namespace Sandbox.Factories
{
    public interface IEmulatorFactory
    {
        Task<IEmulator> CreateEmulatorAsync(string emulatorType);    
    }
}