using Fun;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CosmosChangeFeedTrigger
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

            Console.WriteLine("Creating CosmosClient");

            // Cosmos
            var cosmos = new CosmosClient(config["Cosmos_ConnectionString"]);

            // Fun
            Console.WriteLine("Binding Fun");
            var controller = new FunController();
            var context = new FunContext(controller);

            var fun = new CosmosChangeFeedFunBinding(context, cosmos, config);
            await fun.Bind();

            Console.WriteLine("Ready. Press any key to stop.");

            while (!Console.KeyAvailable) await Task.Delay(1);

            Console.WriteLine("Unbinding Fun.");
            await fun.UnBind();
        }
    }

    public class ToDoItem
    {
        public string id { get; set; }
        internal DateTimeOffset creationTime { get; set; }
    }

    public class ToDoMessage
    {
        public string id { get; set; }
        internal DateTimeOffset creationTime { get; set; }
    }

}
