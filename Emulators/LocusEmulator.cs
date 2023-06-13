
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

    public void HandleTasks(string taskData) //TODO: needs to receive jobId, requestId (and possibly other job data)
    {
        var taskDataObj = JsonConvert.DeserializeObject<TaskData>(taskData);
        var currentTask = taskDataObj.Task;
        var request = JsonConvert.DeserializeObject<OrderJobRequest>(taskDataObj.RequestData);

        //var config = new MapperConfiguration(cfg => { cfg.AddProfile<OrderJobMappingProfile>();});
        //var mapper = config.CreateMapper();
        
        if (currentTask?.TaskType == "PICK") //TODO: maybe make this a switch
        {
            var pickResponse = CreatePickResponse(request, currentTask);
            _rmqProducer.PublishResponse(pickResponse);
            Console.WriteLine("Published Pick Response");
        }
        else if (currentTask?.TaskType == "PACK")
        {
            var pickCompleteResponse = CreatePickCompleteResponse(request);
            _rmqProducer.PublishResponse(pickCompleteResponse);
            Console.WriteLine("Published PickComplete Response");
        }
    }

////////// MAPPING ///////////
    private static Models.JobTasks CreateResponseJobTasks(OrderJobTask currentTask)
    {
        var responseTaskList = new List<OrderJobResultTask>();
        var responseTask = new OrderJobResultTask  //TODO: Map all values
        {
            JobTaskId = currentTask.JobTaskId,
            TaskType = currentTask.TaskType
        };
        responseTaskList.Add(responseTask);

        var jobTasks = new Models.JobTasks 
        {
            OrderJobResultTask = responseTaskList
        };

        return jobTasks;
    }

    private static string CreatePickResponse(OrderJobRequest request, OrderJobTask currentTask) //TODO: or change to passing task with request data (change mapper)
    {
        try
        {
            //var result = mapper.Map<OrderJobResult_Pick_PickComplete>(request);
            var result = new OrderJobResult_Pick_PickComplete
            {
                JobId = request.JobId,
                EventType = "PICK",
                JobStatus = "COMPLETED",
                JobDate = DateTime.UtcNow.ToString(),
                JobStation = "LOCUS",
                ToteId = "T12345", //coordinate with other responses in future
                JobRobot = "P3-001", //coordinate with other responses in future
                JobTasks = CreateResponseJobTasks(currentTask)//TODO: Fix this when figure out job/task passing, for now its mocked
            };
            return JsonConvert.SerializeObject(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("MAPPING ERROR: ", ex.Message);
            return ex.Message;
        }
    }

    private static Models.JobTasks CreateCompleteResponseJobTasks(OrderJobRequest request)
    {
        var responseTaskList = new List<OrderJobResultTask>();

        foreach (var task in request.JobTasks.OrderJobTask)
        {
            var responseTask = new OrderJobResultTask  //TODO: Map all values
            {
                JobTaskId = task.JobTaskId,
                TaskType = task.TaskType
            };
        
            responseTaskList.Add(responseTask);
        }

        var jobTasks = new Models.JobTasks 
        {
            OrderJobResultTask = responseTaskList
        };

        return jobTasks;
    }

    private static string CreatePickCompleteResponse(OrderJobRequest request)
    {
        var result = new OrderJobResult_Pick_PickComplete
        {
            EventType = "PICKCOMPLETE",
            JobStatus = "COMPLETED",
            JobDate = DateTime.UtcNow.ToString(),
            JobStation = "LOCUS",
            ToteId = "T12345", //coordinate with other responses in future
            JobRobot = "P3-001", //coordinate with other responses in future
            JobTasks = CreateCompleteResponseJobTasks(request)
        };
        return JsonConvert.SerializeObject(result);
    }
}