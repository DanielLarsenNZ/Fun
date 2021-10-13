using Azure.Messaging.EventHubs;
using Microsoft.Azure.Cosmos;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

//TODO: Move to Fun.Azure
namespace Fun.Azure
{
    public abstract class EventHubCosmosFunBinding<TInput, TOutput> : FunBinding, IFun<TInput, TOutput>
    {
        private readonly Container _container;

        public EventHubCosmosFunBinding(EventProcessorClient processor, Container container, FunContext context) : base(context)
        {
            EventProcessorClient = processor;
            _container = container;
        }

        public override async Task Bind()
        {
            // Register handlers for processing events and handling errors
            EventProcessorClient.ProcessEventAsync += async (args) =>
            {
                Console.WriteLine("ProcessEventAsync");

                if (args.CancellationToken.IsCancellationRequested) return;

                try
                {
                    await _container.CreateItemAsync(
                        await Run(
                            _context,
                            JsonSerializer.Deserialize<TInput>(Encoding.UTF8.GetString(args.Data.Body.ToArray())), args.CancellationToken));

                    await args.UpdateCheckpointAsync(args.CancellationToken);

                    _context.PostHealth(FunHealth.Normal());
                }
                catch (OutOfMemoryException ex)
                {
                    // TODO: Fix Logging
                    //_context.LogError(ex);
                    _context.ScaleUp(ex);
                    _context.PostHealth(FunHealth.Degraded(ex));
                }
                catch (Exception ex)
                {
                    // TODO: retry, etc
                    Console.WriteLine(ex.Message);
                    _context.PostHealth(FunHealth.Failure(ex));
                }
            };

            EventProcessorClient.ProcessErrorAsync += (args) =>
            {
                Console.WriteLine($"ProcessErrorAsync: {args.Exception.Message}");

                //_context.Logger.LogError(eventArgs.Exception);
                // Circuit breaker
                //if (attempts > 3) Health.Error(ex);
                //else Health.Warning(ex);
                _context.PostHealth(FunHealth.Failure(args.Exception));

                return Task.CompletedTask;
            };

            // Start the processor
            await EventProcessorClient.StartProcessingAsync();

            return;
        }

        public EventProcessorClient EventProcessorClient { get; private set; }

        public override async Task UnBind()
        {
            Console.WriteLine("UnBind");

            await EventProcessorClient.StopProcessingAsync();
        }

        public abstract Task<TOutput> Run(FunContext context, TInput input, CancellationToken cancellationToken);
    }
}
