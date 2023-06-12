
using System.Collections.Concurrent;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sandbox.Configurations;
using Sandbox.Controllers;
using Sandbox.Entities;
using Sandbox.Models;

namespace Sandbox.Emulators;

public class LocusEmulator : IEmulator
{
    private string _jobIdInProcess = "";
    private readonly IRabbitMQProducer _rmqProducer;
    private readonly IRabbitMQConsumer _rmqConsumer;
    private readonly ILogger<LocusEmulator> _logger;

    public LocusEmulator(ILogger<LocusEmulator> logger, IRabbitMQProducer rmqProducer, IRabbitMQConsumer rmqConsumer)
    {
        _rmqProducer = rmqProducer;
        _rmqConsumer = rmqConsumer;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken stoppingToken, int instance)
    {
        await Task.Run(async () =>
        {
            try
            {
                _logger.LogInformation($"LocusEmulator {instance} doing work");
                _rmqConsumer.ConsumeTask((task) => HandleTasks(task));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing messages in EmulatorA.");
            }
        }, stoppingToken);
    }

    public void HandleTasks(string task)
    {
        var currentTask = JsonConvert.DeserializeObject<OrderJobTask>(task);

        if (currentTask.TaskType == "PICK") //maybe make this a switch
        {
            //var pickResponse = CreatePickResponse(mapper, request);
            _rmqProducer.PublishResponse("pickresponse");
            Console.WriteLine("Published Pick Response");
        }
        else if (currentTask.TaskType == "PACK")
        {
            //var pickCompleteResponse = CreatePickCompleteResponse(mapper, request);
            _rmqProducer.PublishResponse("pickCompleteResponse");
            Console.WriteLine("Published PickComplete Response");
        }
    }

    private static string CreatePickResponse(IMapper mapper, OrderJobRequest request)
    {
        var result = mapper.Map<OrderJobResult_Pick_PickComplete>(request);

        result.EventType = "PICK";
        result.JobStatus = "COMPLETED";
        result.JobDate = DateTime.UtcNow.ToString();
        result.JobStation = "LOCUS";
        result.ToteId = "T12345"; //coordinate with other responses in future
        result.JobRobot = "P3-001"; //coordinate with other responses in future
        return JsonConvert.SerializeObject(result);
    }
}