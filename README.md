# Fun

Like Lambda and Functions, but Fun<sup>*</sup>.

## Getting started

Implement a micro-service in a functional style as `IFun`

```csharp
public class TabulateEvents : IFun
{
    public Task<MyDocument> Run (FunContext context, MyEvent input)
    {
        context.Logger.LogInformation(input.Description);

        if (input.Type == "Add") return new MyDocument{ Sum = input.Value1 + input Value2 };
        else return new MyDocument{ Sum = input.Value1 - input Value2 };
    } 
}
```

Implement a binding by inheriting from `FunBinding`

```csharp
public class MyBinding : FunBinding
{
    public override async Task Bind() 
    {
        //...
    }
}
```

Map Functions to Bindings

```csharp
app.UseFun<TabulateEvents, MyBinding>();
```

Build as a console exe and run anywhere that .NET Core is supported.

## Fun

> `IFun` promotes SOLID principles and Functional programming techniques; single input and output types (can be complex types) give better closure and promote single responsibility principle. Fun services are designed to contain only a few lines of business logic code. 

```csharp
public interface IFun
{
    Task<TOutput> Run(FunContext context, TInput input);
}

// e.g.
public class TabulateEvents : IFun
{
    public Task<MyDocument> Run (FunContext context, MyEvent input)
    {
        context.Logger.LogInformation(input.Description);

        if (input.Type == "Add") return new MyDocument{ Sum = input.Value1 + input Value2 };
        else return new MyDocument{ Sum = input.Value1 - input Value2 };
    } 
}
```

## Bindings

Bindings are kept strictly separate; no leaky binding abstraction. Build on a library of generic bindings or build your own:

```csharp
// Saves an Event received from an Event Hub to Cosmos DB. 
// Event deserialization and Document serialization is implemented in the generic Binding base class.
// Config and context are injected as dependencies.
public class SaveEventFun : EventHubCosmosFunBinding<MyEvent, MyDocument>
{
    public override Task<MyDocument> Run(FunContext context, MyEvent input)
    {
        try
        {
            return Task.FromResult(
                new MyDocument { 
                    Id = Guid.NewGuid().ToString("N"), 
                    MyProperty = input.MyProperty });
        }
        catch (Exception ex)
        {
            context.Logger.LogError(ex);
            context.PostHealth(FunHealth.Failure(ex));
            throw;
        }
    }
}
```

Build your own Binding by inheriting from `FunBinding`

```csharp
public class MyBinding : FunBinding
{
    public override async Task Bind() 
    {
        //...
    }
}
```

## Scale controllers

Build your own Scale controller by inheriting from `FunController`

```csharp
public class FunDaprController : FunController
{
    public override async Task ScaleUp() 
    {
        //...
    }

    public override async Task ScaleDown()
    {
        //...
    }
}
```

Functions or Bindings can request Scale up or down:

```csharp
catch (OutOfMemoryException ex)
{
    FunContext.Logger.LogError(ex);
    FunContext.ScaleUp(ex);
    FunContext.PostHealth(FunHealth.Degraded(ex));
}
```

## FunContext

Health, Scale, Telemetry, Authorization, Logging and Configuration are first class citizens with properties and methods on `FunContext`

```csharp
public virtual Task Authorize();

public virtual Task<FunHealth> GetHealth();

public virtual void PostHealth(FunHealth health);

public virtual void ScaleUp(object metadata);

public virtual void ScaleDown(object metadata);

public ILogger Logger { get; }

public ITelemetry Telemetry { get; }

public IConfiguration Configuration { get; }
```

## HTTP Functions

Extension methods for ASP.NET Core

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddFun<HelloFun>();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseRouting();
    app.UseFunPost<HelloFun>("/");
}
```

Easy to build on `HttpFunBinding`:

```csharp
public class HelloFun : HttpFunBinding<MyModel, MyModel>
{
    public override Task<MyModel> Run(FunContext context, MyModel input)
    {
        return Task.FromResult(input);
    }
}
```

## Rationale

<sup>*</sup>This prototype looks to improve on some things that are not awesome in Lambda and Functions today:

* Bindings are opaque and out of Users' (Developers') control
* Scaling is opaque and out of Users' control
* Bindings leak into Function code
* Triggers are opaque, confusing, and leak into Function code
* Telemetry is not first class and/or is tied to vendors' services
* Health is not first class; Functions cannot report on their own health
* Surpisingly not super easy to run in Containers. Not easily portable
* IoC is incomplete and inconsistent
* Authorization is not first-class

Contributions, feedback and issues welcome!
