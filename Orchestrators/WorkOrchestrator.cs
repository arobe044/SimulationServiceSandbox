using System.Text;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sandbox.Controllers;
using Sandbox.Entities;
using Sandbox.Models;

namespace Sandbox.Orchestrators
{
    public class WorkOrchestrator : BackgroundService //TODO: Genericize this to get current emulator type (replace config key below)
    {
        private readonly IRabbitMQConsumer _rmqConsumer;
        private readonly IRabbitMQProducer _rmqProducer;

        public WorkOrchestrator(IRabbitMQConsumer rmqConsumer, 
                                IRabbitMQProducer rmqProducer
                                )
        {
            _rmqConsumer = rmqConsumer;
            _rmqProducer = rmqProducer;
        }

        public void ProcessRequest(string request)
        {
            try
            {
                if (IsWorkAuthorized())
                {   
                    /////////////////
                    ///TODO: either (ideally) would move these to inside emulator once Job/Task queue figured out 
                    /// (currently scoped to task so repeat responses would occur)
                    /// OR change CreateLocusTaskList to receive deserialized request (avoid deserializing twice)
                    var requestObject = JsonConvert.DeserializeObject<OrderJobRequest>(request);
                    CreateAcceptedResponse(requestObject);
                    CreateToteInductResponse(requestObject); 
                    /////////////////

                    CreateLocusTaskList(request);
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
            var requestObject = JsonConvert.DeserializeObject<OrderJobRequest>(request);

            List<OrderJobTask> OrderJobTasks = requestObject.JobTasks.OrderJobTask;
            foreach (var task in OrderJobTasks)
            {
                var serializedTask = JsonConvert.SerializeObject(task);
                _rmqProducer.PublishTask(serializedTask);
                
                Console.WriteLine($"Published Task {task.TaskType} to TaskList for Locus Emulator");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) //Entry point on startup
        {
            await Task.Run(async () =>
            {
                Console.WriteLine("Orchestrator consuming tasks");
                _rmqConsumer.ConsumeRequest((message) => ProcessRequest(message));
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
}

