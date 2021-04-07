using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using Fun;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("ConsoleApp1 is starting");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("Creating Storage Client");

            // Storage
            var storageClient = new BlobContainerClient(config["Blob_ConnectionString"], config["Blob_Container"]);

            Console.WriteLine("Creating EventProcessorClient");

            // Event Hubs
            var processor = new EventProcessorClient
            (
                storageClient,
                "$Default",
                config["EventHub_ConnectionString"],
                config["EventHub_HubName"]
            );

            Console.WriteLine("Creating CosmosClient");

            // Cosmos
            var cosmos = new CosmosClient(config["Cosmos_ConnectionString"]);
            var cosmosContainer = cosmos.GetContainer(config["Cosmos_DatabaseName"], config["Cosmos_ContainerName"]);

            // Fun
            Console.WriteLine("Binding Fun");
            var controller = new FunController();
            var context = new FunContext(controller);

            var fun = new EventHubCosmosFun(processor, cosmosContainer, context);
            await fun.Bind();

            Console.WriteLine("Ready. Press any key to stop.");
            
            while (!Console.KeyAvailable) await Task.Delay(1);

            Console.WriteLine("Unbinding Fun.");
            await fun.UnBind();
        }
    }
}
