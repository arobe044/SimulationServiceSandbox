using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sandbox.Configurations;
using Sandbox.Factories;
using Sandbox.Services;
using Sandbox.Controllers;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args); 
        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", optional:false, reloadOnChange: true).Build();
        var rabbitMQConfig = config.GetSection("RabbitMQConfig").Get<RabbitMQConfig>();
        var emulatorConfigList = config.GetSection("EmulatorConfig").Get<IDictionary<string, EmulatorConfig>>();

        builder.ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton(rabbitMQConfig);
            services.AddSingleton(emulatorConfigList);
            services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
            services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
            services.AddSingleton<IEmulatorFactory, EmulatorFactory>();
            services.AddHostedService<EmulatorService>();
            services.AddHostedService<WorkOrchestrator>();
        });

        var host = builder.Build();
        host.Start();
        // Wait for the host to shutdown
        host.WaitForShutdown();
    }
}