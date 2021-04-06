# Fun

Like Lambda and Functions, but Fun. 

## Getting started

```csharp
// Implement this
public interface IFun
{
    TOutput Run<TInput, TOutput> (FunContext context, TInput input);
}

// or this
public interface IAsyncFun
{
    Task<TOutput> Run<TInput, TOutput> (FunContext context, TInput input);
}

public class FilterEvents : IFun
{
    // Should we force Run to be synchronous?
    public MyDocument Run (FunContext context, MyEvent input)
    {
        if (input.Type == 1) return new MyDocument(input);
    } 
}
```

## FunContext

```csharp
public class FunContext
{
    public FunContext (FunController controller, ILogger logger, Telemetry telemetry);

    Task Bind (FunBindingContext context);

    Task UnBind();

    Task<FunHealth> Health ();

    public Task ScaleUp (object metadata) => controller.ScaleUp();

    public Task ScaleDown (object metadata) => controller.ScaleDown();

    public ILogger Logger { get; }

    public Telemetry Telemetry { get; }

    public FunHealth Health { get; } 
}
```

## Bind it yourself

```csharp
public class EventToCosmosFun : IFun
{
    private readonly EventProcessorClient _processor;
    private readonly Container _container;
    private bool _unbinding = false;

    public EventFun (EventProcessorClient processor, Container container)
    {
        _processor = processor;
        _container = container;
    }

    public async Task Bind (FunStartupContext context)
    {
        // Register handlers for processing events and handling errors
        _processor.ProcessEventAsync += new (ProcessEventArgs eventArgs)
        {
            if (args.CancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            try
            {
                await _container.CreateItemAsync(
                    Run<MyEvent, MyDocument>(
                        JsonConvert.DeserializeObject<MyEvent>(
                            Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()))));
                
                await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
                Health.Ok();
            }
            catch (OutOfMemoryException ex)
            {
                context.Logger.LogError(ex);
                ScaleUp(ex);
            }
            catch (Exception ex)
            {
                // TODO: retry logic
                context.Logger.LogError(ex);

                // Circuit breaker
                if (attempts > 3) Health.Error(ex);
                else Health.Warning(ex);
            }
        };

        _processor.ProcessErrorAsync += new (ProcessErrorEventArgs eventArgs) 
        {
            context.Logger.LogError(eventArgs.Exception);
            // Circuit breaker
            if (attempts > 3) Health.Error(ex);
            else Health.Warning(ex);
        }

        // Start the processing
        await _processor.StartProcessingAsync();

        while (!_unbinding) Task.Delay(1);
    }

    public async Task UnBind ()
    {
        _unbinding = true;
        await _processor.StopProcessingAsync();
    }

    public async Task<FunHealth> Health() => Health;
}
```

## Scale Controllers

```csharp
public class KubernetesFunController : FunController
{
    Task ScaleUp(object metadata);
    Task ScaleDown(object metadata);
}
```