using System.Security.AccessControl;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sandbox.Configurations;

namespace Sandbox.Controllers;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly RabbitMQConfig _rmqConfig;
    private readonly IDictionary<string,EmulatorConfig> _emConfigList;
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _publisherConnection;
    private readonly IModel _requestPublishChannel;
    private readonly IModel _responsePublishChannel;
    private readonly IModel _taskPublishChannel;

    public RabbitMQProducer(RabbitMQConfig rmqConfig, IDictionary<string,EmulatorConfig> emConfigList)
    {      
        _rmqConfig = rmqConfig;
        _emConfigList = emConfigList;
        _connectionFactory = new ConnectionFactory() { HostName = _rmqConfig.HostName, Port = _rmqConfig.Port, UserName = _rmqConfig.Username, Password = _rmqConfig.Password };
        _publisherConnection = _connectionFactory.CreateConnection();
        _requestPublishChannel = _publisherConnection.CreateModel();
        _responsePublishChannel = _publisherConnection.CreateModel();
        _taskPublishChannel = _publisherConnection.CreateModel();
    }
    
    public void PublishTask(string serializedTask)
    {
        try
        {
            if (serializedTask is not null) 
            {
                var task = Encoding.UTF8.GetBytes(serializedTask);
                _taskPublishChannel.BasicPublish(   _rmqConfig.TaskExchangeName, 
                                                    _emConfigList["LocusEmulator"].TaskRoutingKey, 
                                                    null, task  );
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error publishing task: ", ex);
        }
    }

    public void PublishResponse(string serializedResponse)
    {
        try
        {
            if (serializedResponse is not null) 
            {
                var response = Encoding.UTF8.GetBytes(serializedResponse);
                _responsePublishChannel.BasicPublish(   _rmqConfig.ResponseExchangeName, 
                                                        _emConfigList["LocusEmulator"].ResponseRoutingKey, 
                                                        null, response  );
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error publishing response: ", ex);
        }
    }

    public void PublishRequest(string serializedRequest)
    {
        //In the case that the emulator is adding a job to another emulator's request queue
        try
        {
            if (serializedRequest is not null) 
            {
                var request = Encoding.UTF8.GetBytes(serializedRequest);
                _requestPublishChannel.BasicPublish(    _rmqConfig.RequestExchangeName, 
                                                        _emConfigList["LocusEmulator"].RequestRoutingKey, 
                                                        null, request   );
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error publishing new request: ", ex);
        }
    }

    public void StopPublishing()
    {
        //close channels
        //close connection
        //writeline stopped publishing
    }
}