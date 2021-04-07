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
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();


            // Storage
            var storageClient = new BlobContainerClient(config["Blob_ConnectionString"], config["Blob_Container"]);

            // Event Hubs
            var processor = new EventProcessorClient
            (
                storageClient,
                "_default",
                config["EventHub_ConnectionString"],
                config["EventHub_HubName"]
            );

            // Cosmos
            var cosmos = new CosmosClient(config["Cosmos_ConnectionString"]);
            var cosmosContainer = cosmos.GetContainer(config["Cosmos_DatabaseName"], config["Cosmos_ContainerName"]);

            var controller = new FunController();
            var context = new FunContext(controller);

            var fun = new EventHubCosmosFun(processor, cosmosContainer, context);
            await fun.Bind();

            bool unbinding = false;
            while (!unbinding) await Task.Delay(1);

            await fun.UnBind();
        }
    }
}
