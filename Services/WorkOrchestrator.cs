using System.Text;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sandbox.Controllers;
using Sandbox.Entities;
using Sandbox.Models;

namespace Sandbox.Services;

public class WorkOrchestrator : BackgroundService //TODO: Genericize this to get current emulator type (replace config key below)
{
    private readonly IRabbitMQConsumer _rmqConsumer;
    private readonly IRabbitMQPublisher _rmqPublisher;

    public WorkOrchestrator(IRabbitMQConsumer rmqConsumer, IRabbitMQPublisher rmqPublisher)
    {
        _rmqConsumer = rmqConsumer;
        _rmqPublisher = rmqPublisher;
    }

    public void ProcessRequest(string request)
    {
        try
        {
            if (IsWorkAuthorized())
            {   
                CreateLocusTaskList(request); //TODO: Genericize
            }
            else
            {
                //  no op the work
                //  send some kind of response?
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Processing Request: ", ex.Message);
        }
    }

    private void CreateLocusTaskList(string request)
    {
        var emulatorType = "LocusEmulator";
        var requestObject = JsonConvert.DeserializeObject<OrderJobRequest>(request);

        //TODO: (ideally) would move these to inside emulator, but emulator is triggered by tasks, so currrent architecture would cause duplicate accepted and toteinduct responses
        var acceptedResponse = CreateAcceptedResponse(requestObject);
        _rmqPublisher.PublishResponse(emulatorType, acceptedResponse);
        Console.WriteLine("Published RESPONSE: OrderJob Accepted Result");

        var inductedResponse = CreateToteInductResponse(requestObject);
        _rmqPublisher.PublishResponse(emulatorType, inductedResponse);
        Console.WriteLine("Published RESPONSE: OrderJob ToteInduct Result");

        List<OrderJobTask> OrderJobTasks = requestObject.JobTasks.OrderJobTask;
        foreach (var task in OrderJobTasks)
        {
            var taskData = new TaskData
            {
                Task = task,
                RequestData = request
            };

            var serializedTask = JsonConvert.SerializeObject(taskData);
            
            _rmqPublisher.PublishTask(emulatorType, serializedTask);
            
            Console.WriteLine($"Published TASK: {task.TaskType} to TaskList for Locus Emulator");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) //Entry point on startup
    {
        await Task.Run(async () =>
        {
            var emulatorType = "LocusEmulator";
            //TODO: loop for all emulator types, consume queues
            Console.WriteLine("Work Orchestrator consuming requests");
            _rmqConsumer.ConsumeRequest(emulatorType, (message) => ProcessRequest(message));
        }, stoppingToken);
    }

    private bool IsWorkAuthorized(/*string jobID*/)
    {
        //  if jobId is not in cache or jobstatus is 'canceling' (job is being canceled, was canceled, or was finished already)
        //  return false
        //  if jobId is in cache
        return true;
    }

    ////////MAPPING
    private static string CreateAcceptedResponse(OrderJobRequest request)
    {
        //var result = mapper.Map<OrderJobResult_Accept>(request);
        var result = new OrderJobResult_Accept
        {
            EventType = "ACCEPT",
            JobStatus = "COMPLETED",
            JobDate = DateTime.UtcNow.ToString(),
            RequestId = request.RequestId
        };
        return JsonConvert.SerializeObject(result);
    }

    private static string CreateToteInductResponse(OrderJobRequest request)
    {
        //var result = mapper.Map<OrderJobResult_ToteInduct>(request);
        var result = new OrderJobResult_ToteInduct
        {
            EventType = "TOTEINDUCT",
            JobStatus = "COMPLETED",
            JobDate = DateTime.UtcNow.ToString(),
            ToteId = "T12345", //coordinate with other responses in future
            JobRobot = "P3-001", //coordinate with other responses in future
            JobId = request.JobId
        };
        return JsonConvert.SerializeObject(result);
    }
}