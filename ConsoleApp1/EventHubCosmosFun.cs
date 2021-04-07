using Azure.Messaging.EventHubs;
using Fun;
using Microsoft.Azure.Cosmos;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class EventHubCosmosFun : IFun<MyEvent, MyDocument>
    {
        private readonly Container _container;
        private bool _unbinding = false;
        private readonly FunContext _context;

        public EventHubCosmosFun(EventProcessorClient processor, Container container, FunContext context)
        {
            EventProcessorClient = processor;
            _container = container;
            _context = context;
        }

        public Task<MyDocument> Run(FunContext context, MyEvent input)
        {
            return Task.FromResult(new MyDocument { MyProperty = input.MyProperty });
        }

        public async Task Bind()
        {
            // Register handlers for processing events and handling errors
            EventProcessorClient.ProcessEventAsync += async (args) =>
            {
                if (args.CancellationToken.IsCancellationRequested) return;

                try
                {
                    await _container.CreateItemAsync(
                        await Run(
                            _context,
                            JsonSerializer.Deserialize<MyEvent>(Encoding.UTF8.GetString(args.Data.Body.ToArray()))));

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
                    _context.PostHealth(FunHealth.Failure(ex));
                }
            };

            EventProcessorClient.ProcessErrorAsync += async (args) =>
            {
                //_context.Logger.LogError(eventArgs.Exception);
                // Circuit breaker
                //if (attempts > 3) Health.Error(ex);
                //else Health.Warning(ex);
                _context.PostHealth(FunHealth.Failure(args.Exception));
            };

            // Start the processing
            await EventProcessorClient.StartProcessingAsync();

            return;
        }

        public EventProcessorClient EventProcessorClient { get; private set; }

        public async Task UnBind()
        {
            _unbinding = true;
            await EventProcessorClient.StopProcessingAsync();
        }
    }
}
