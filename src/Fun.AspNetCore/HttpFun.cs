using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Fun.AspNetCore
{
    public abstract class HttpFun<TInput, TOutput> : Fun<TInput, TOutput>, IHttpFun //, IFun<TInput, TOutput>
    {
        public HttpFun(FunContext context) : base(context) { }

        public override Task Bind() => Task.FromResult(
            RequestDelegate = new RequestDelegate(
                async (context) =>
                {
                    switch (context.Request.Method.ToUpper())
                    {
                        case "POST":
                        case "PUT":
                        case "PATCH":
                            await context.Response.WriteAsJsonAsync(await Run(_context, await context.Request.ReadFromJsonAsync<TInput>()));
                            break;
                        default:
                            //TODO: Query string binding
                            throw new NotSupportedException($"HTTP {context.Request.Method.ToUpper()} is not currently supported.");
                    }
                }));

        public RequestDelegate RequestDelegate { get; protected set; }
    }

    public interface IHttpFun : IFun
    {
        RequestDelegate RequestDelegate { get; }
    }
}
