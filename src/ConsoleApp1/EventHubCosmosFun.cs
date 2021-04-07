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
        private readonly FunContext _context;

        public EventHubCosmosFun(EventProcessorClient processor, Container container, FunContext context)
        {
            EventProcessorClient = processor;
            _container = container;
            _context = context;
        }

        public Task<MyDocument> Run(FunContext context, MyEvent input)
        {
            try
            {
                Console.WriteLine("Run");
                return Task.FromResult(new MyDocument { Id = Guid.NewGuid().ToString("N"), MyProperty = input.MyProperty });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Bind()
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

        public async Task UnBind()
        {
            Console.WriteLine("UnBind");

            await EventProcessorClient.StopProcessingAsync();
        }
    }
}
