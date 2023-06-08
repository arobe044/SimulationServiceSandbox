
using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sandbox.Configurations;

namespace Sandbox.Workers;

public class WorkerA : IWorker
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMQConfig _rmqConfig;
    private readonly ILogger<WorkerA> _logger;

    public WorkerA(ILogger<WorkerA> logger, RabbitMQConfig rmqConfig)
    {
        _rmqConfig = rmqConfig;
        _connectionFactory = new ConnectionFactory() { HostName = _rmqConfig.HostName, Port = _rmqConfig.Port, UserName = _rmqConfig.Username, Password = _rmqConfig.Password };
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;
    }

    public async Task DoWorkAsync(CancellationToken stoppingToken, int instance)
    {
        await Task.Run(async () =>
        {
            try
            {
                _logger.LogInformation($"WorkerA {instance} doing work");
            
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"WorkerA {instance} processing message");
                                
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                };
                _channel.BasicConsume(queue: "sim_service_requests_demo_queue", autoAck: false, consumer: consumer);

                //_channel.BasicCancel(consumer.ConsumerTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing messages in WorkerA.");
            }
        }, stoppingToken);
    }
}