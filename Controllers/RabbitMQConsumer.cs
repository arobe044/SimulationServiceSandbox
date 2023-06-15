using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sandbox.Configurations;
using Sandbox.Entities;

namespace Sandbox.Controllers;

public class RabbitMQConsumer : IRabbitMQConsumer
{
    private readonly RabbitMQConfig _rmqConfig;
    private readonly IDictionary<string,EmulatorConfig> _emConfigList;
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _consumerConnection;
    private readonly IModel _requestConsumeChannel;
    private readonly IModel _taskConsumeChannel;

    public RabbitMQConsumer(RabbitMQConfig rmqConfig, IDictionary<string,EmulatorConfig> emConfigList)
    {      
        _rmqConfig = rmqConfig;
        _emConfigList = emConfigList;
        _connectionFactory = new ConnectionFactory() { HostName = _rmqConfig.HostName, Port = _rmqConfig.Port, UserName = _rmqConfig.Username, Password = _rmqConfig.Password };
        _consumerConnection = _connectionFactory.CreateConnection();
        _requestConsumeChannel = _consumerConnection.CreateModel();
        _taskConsumeChannel = _consumerConnection.CreateModel();
    }

    public void ConsumeRequest(string emulatorType, Action<string> messageHandler)
    {
        try
        {
            //TODO: Openapi for this and queues?
            _requestConsumeChannel.ExchangeDeclare(exchange:_rmqConfig.RequestExchange, type: ExchangeType.Topic, durable:true);
            _requestConsumeChannel.QueueDeclare(_emConfigList[emulatorType].RequestQueue, true, false, false, null);
            _requestConsumeChannel.QueueBind(queue:_emConfigList[emulatorType].RequestQueue, exchange: _rmqConfig.RequestExchange, routingKey: _emConfigList[emulatorType].RequestRoutingKey);

            var consumer = new EventingBasicConsumer(_requestConsumeChannel);
            consumer.Received += (channel, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                messageHandler(message);
                _requestConsumeChannel.BasicAck(ea.DeliveryTag, false);
            };
            string consumerTag = _requestConsumeChannel.BasicConsume(_emConfigList[emulatorType].RequestQueue, false, consumer);
            Console.WriteLine($"Listening for messages on {emulatorType} request queue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error RMQConsumer with request: ", ex);
        }
    }

    //CHANGE TO ONE AT A TIME
    public void ConsumeTask(Action<string> messageHandler)
    {
        try
        {
            var consumer = new EventingBasicConsumer(_taskConsumeChannel);
            consumer.Received += (channel, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                messageHandler(message);
                _taskConsumeChannel.BasicAck(ea.DeliveryTag, false);
            };
            _taskConsumeChannel.BasicConsume(_emConfigList["LocusEmulator"].TaskQueue, false, consumer);
            Console.WriteLine($"Listening for messages on task queue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error RMQConsumer with task: ", ex);
        }
    }

    public void StopConsuming(string consumerTag)
    {
        //basicCancel(consumerTag)
        //close channels
        //close connection
        //writeline stopped consuming
    }
}