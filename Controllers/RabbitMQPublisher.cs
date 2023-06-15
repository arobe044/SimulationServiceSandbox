using System.Text;
using RabbitMQ.Client;
using Sandbox.Configurations;

namespace Sandbox.Controllers;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly RabbitMQConfig _rmqConfig;
    private readonly IDictionary<string,EmulatorConfig> _emConfigList;
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _publisherConnection;
    private readonly IModel _requestPublishChannel;
    private readonly IModel _responsePublishChannel;
    private readonly IModel _taskPublishChannel;

    public RabbitMQPublisher(RabbitMQConfig rmqConfig, IDictionary<string,EmulatorConfig> emConfigList)
    {      
        _rmqConfig = rmqConfig;
        _emConfigList = emConfigList;
        _connectionFactory = new ConnectionFactory() { HostName = _rmqConfig.HostName, Port = _rmqConfig.Port, UserName = _rmqConfig.Username, Password = _rmqConfig.Password };
        _publisherConnection = _connectionFactory.CreateConnection();
        _requestPublishChannel = _publisherConnection.CreateModel();
        _responsePublishChannel = _publisherConnection.CreateModel();
        _taskPublishChannel = _publisherConnection.CreateModel();
    }
    
    public void BuildTaskQueue(string emulatorType)
    {
        _taskPublishChannel.ExchangeDeclare(exchange:_rmqConfig.TaskExchange, type: ExchangeType.Topic, durable:true);
        _taskPublishChannel.QueueDeclare(_emConfigList[emulatorType].TaskQueue, true, false, false, null);
        _taskPublishChannel.QueueBind(queue:_emConfigList[emulatorType].TaskQueue, exchange: _rmqConfig.TaskExchange, routingKey: _emConfigList[emulatorType].TaskRoutingKey);
    }

    public void PublishTask(string emulatorType, string serializedTask)
    {
        try
        {
            //declare exchange and bind queue

            if (serializedTask is not null) 
            {
                var task = Encoding.UTF8.GetBytes(serializedTask);
                _taskPublishChannel.BasicPublish(   _rmqConfig.TaskExchange, 
                                                    _emConfigList[emulatorType].TaskRoutingKey, 
                                                    null, task  );
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error publishing task: ", ex);
        }
    }

    public void PublishResponse(string emulatorType, string serializedResponse)
    {
        try
        {
            //declare exchange and bind queue

            if (serializedResponse is not null) 
            {
                var response = Encoding.UTF8.GetBytes(serializedResponse);
                _responsePublishChannel.BasicPublish(   _rmqConfig.ResponseExchange, 
                                                        _emConfigList[emulatorType].ResponseRoutingKey, 
                                                        null, response  );
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing {emulatorType} response: ", ex);
        }
    }

    public void PublishRequest(string emulatorType, string serializedRequest)
    {
        //In the case that the emulator is adding a job to another emulator's request queue
        try
        {
            if (serializedRequest is not null) 
            {
                var request = Encoding.UTF8.GetBytes(serializedRequest);
                _requestPublishChannel.BasicPublish(    _rmqConfig.RequestExchange,
                                                        _emConfigList[emulatorType].RequestRoutingKey, 
                                                        null, request   );
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error publishing new request: ", ex.Message);
        }
    }

    public void StopPublishing()
    {
        //close channels
        //close connection
        //writeline stopped publishing
    }
}