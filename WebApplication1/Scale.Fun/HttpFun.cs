using Microsoft.AspNetCore.Http;
using Scale.Fun;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Scale.Fun
{
    public abstract class HttpFun<TInput, TOutput> : IHttpFun, IFun<TInput, TOutput>
    {
        protected readonly FunContext _context;

        public HttpFun(FunContext context) => _context = context;

        public abstract Task<TOutput> Run(FunContext context, TInput input);

        public Task Bind() => Task.FromResult(RequestDelegate = new RequestDelegate(CreateRequestDelegate<TInput>()));

        private Func<HttpContext, Task> CreateRequestDelegate<T> () 
            => async (context) => await context.Response.WriteAsJsonAsync(
                await Run(_context, await context.Request.ReadFromJsonAsync<TInput>()));

        public RequestDelegate RequestDelegate { get; protected set; }
    }

    public interface IHttpFun : IFun
    {
        RequestDelegate RequestDelegate { get; }
    }
}
