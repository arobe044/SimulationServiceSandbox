using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sandbox;
using Sandbox.Configurations;
using Sandbox.Factories;
using Newtonsoft.Json;
using Sandbox.Orchestrators;
using Sandbox.Controllers;

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
            services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddSingleton<IWorkerFactory, WorkerFactory>();
            services.AddHostedService<WorkerService>();
            services.AddHostedService<WorkOrchestrator>();
        });

        var host = builder.Build();
        host.Start();
        // Wait for the host to shutdown
        host.WaitForShutdown();
    }
}