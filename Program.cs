using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sandbox;
using Sandbox.Configurations;
using Sandbox.Factories;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args); 
        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", optional:false, reloadOnChange: true).Build();
        var rabbitMQConfig = config.GetSection("RabbitMQConfig").Get<RabbitMQConfig>();
        var workerConfigList = config.GetSection("WorkerConfig").Get<IDictionary<string, WorkerConfig>>();

        builder.ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton(rabbitMQConfig);
            services.AddSingleton(workerConfigList);
            services.AddSingleton<IWorkerFactory, WorkerFactory>();
            services.AddHostedService<WorkerService>();
        });

        var host = builder.Build();
        host.Start();
        // Wait for the host to shutdown
        host.WaitForShutdown();
    }
}

//class Program
//{
    //static void Main(string[] args)
    //{
        // var configuration = new ConfigurationBuilder()
        //     .AddJsonFile("appsettings.json")
        //     .Build();
        // var workerConfig = configuration.GetSection("WorkerConfig").Get<WorkerConfig>();
        // var cancellationTokenSource = new CancellationTokenSource();
        
        // // Create the worker factory
        // var workerFactory = new WorkerFactory();
        
        // // Start the workers
        // foreach (var workerType in workerConfig.WorkerTypes)
        // {
        //     for (int i = 0; i < workerType.Instances; i++)
        //     {
        //         var worker = workerFactory.CreateWorker(workerType.Type, new ConcurrentQueue<string>(), cancellationTokenSource.Token);
        //         Task.Run(() => worker.Start());
        //     }
        // }
        // // Add messages to the queue
        // // ...
        // // Wait for the workers to finish processing messages
        // cancellationTokenSource.Cancel();
        // Console.ReadLine();
    //}
//}