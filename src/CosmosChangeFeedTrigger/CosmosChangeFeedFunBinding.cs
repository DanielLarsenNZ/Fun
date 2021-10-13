using Fun;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosChangeFeedTrigger
{
    public class CosmosChangeFeedFunBinding : FunBinding, IFun<IReadOnlyCollection<ToDoItem>, IEnumerable<ToDoMessage>>
    {
        private readonly CosmosClient _cosmos;
        private readonly IConfiguration _config;

        public CosmosChangeFeedFunBinding(FunContext context, CosmosClient cosmos, IConfiguration config) : base(context)
        {
            _cosmos = cosmos;
            _config = config;
        }
        public ChangeFeedProcessor ChangeFeedProcessor { get; private set; }

        public override async Task Bind()
        {
            ChangeFeedProcessor = await StartChangeFeedProcessorAsync(_cosmos, _config);
        }

        public override async Task UnBind()
        {
            Console.WriteLine("UnBind");
            await ChangeFeedProcessor.StopAsync();
        }

        public async Task<IEnumerable<ToDoMessage>> Run(
            FunContext context, 
            IReadOnlyCollection<ToDoItem> input, 
            CancellationToken cancellationToken) 
            => input.Select(i => new ToDoMessage { id = i.id, creationTime = i.creationTime });

        /// <summary>
        /// Start the Change Feed Processor to listen for changes and process them with the HandleChangesAsync implementation.
        /// </summary>
        private static async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync(
            CosmosClient cosmosClient,
            IConfiguration configuration)
        {
            string databaseName = configuration["SourceDatabaseName"];
            string sourceContainerName = configuration["SourceContainerName"];
            string leaseContainerName = configuration["LeasesContainerName"];

            Container leaseContainer = cosmosClient.GetContainer(databaseName, leaseContainerName);
            ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(databaseName, sourceContainerName)
                .GetChangeFeedProcessorBuilder<ToDoItem>(processorName: "changeFeedSample", onChangesDelegate: HandleChangesAsync)
                    .WithInstanceName("consoleHost")
                    .WithLeaseContainer(leaseContainer)
                    .Build();

            Console.WriteLine("Starting Change Feed Processor...");
            await changeFeedProcessor.StartAsync();
            Console.WriteLine("Change Feed Processor started.");
            return changeFeedProcessor;
        }

        /// <summary>
        /// The delegate receives batches of changes as they are generated in the change feed and can process them.
        /// </summary>
        private static async Task HandleChangesAsync(
            IReadOnlyCollection<ToDoItem> changes,
            CancellationToken cancellationToken)
        {

        }
    }
}
