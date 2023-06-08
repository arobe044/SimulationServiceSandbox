using Sandbox.Workers;

namespace Sandbox.Factories
{
    public interface IWorkerFactory
    {
        Task<IWorker> CreateWorkerAsync(string workerType);    
    }
}