using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fun.AspNetCore
{
    public abstract class HttpFunBinding<TInput, TOutput> : FunBinding, IHttpFunBinding, IFun<TInput, TOutput>
    {
        public HttpFunBinding(FunContext context) : base(context) { }

        public override Task Bind() 
            => Task.FromResult(
                RequestDelegate = new RequestDelegate(
                    async (context) =>
                    {
                        switch (context.Request.Method.ToUpper())
                        {
                            case "POST":
                            case "PUT":
                            case "PATCH":
                                await context.Response.WriteAsJsonAsync(await Run(_context, await context.Request.ReadFromJsonAsync<TInput>(), context.RequestAborted));
                                break;
                            default:
                                //TODO: Query string binding
                                throw new NotSupportedException($"HTTP {context.Request.Method.ToUpper()} is not currently supported.");
                        }
                    }));

        public abstract Task<TOutput> Run(FunContext context, TInput input, CancellationToken cancellationToken);

        public RequestDelegate RequestDelegate { get; protected set; }
    }

    public interface IHttpFunBinding : IFunBinding
    {
        RequestDelegate RequestDelegate { get; }
    }
}
