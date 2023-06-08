using System.Text;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sandbox.Controllers;
using Sandbox.Entities;

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
                    CreateTaskList(request);
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

        private void CreateTaskList(string request)
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
    }
}

