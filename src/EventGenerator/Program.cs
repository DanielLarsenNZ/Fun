using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            // Create a producer client that you can use to send events to an event hub
            await using (var producerClient = new EventHubProducerClient(config["EventHub_ConnectionString"], config["EventHub_HubName"]))
            {
                while (!Console.KeyAvailable)
                {
                    // Create a batch of events 
                    using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                    // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                    eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new MyEvent { MyProperty = 1 }))));
                    eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new MyEvent { MyProperty = 2 }))));
                    eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new MyEvent { MyProperty = 3 }))));

                    // Use the producer client to send the batch of events to the event hub
                    await producerClient.SendAsync(eventBatch);
                    Console.WriteLine("A batch of 3 events has been published.");
                    await Task.Delay(5000);
                }
            }
        }
    }
}
