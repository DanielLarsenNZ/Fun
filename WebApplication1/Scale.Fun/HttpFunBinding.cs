//using Microsoft.AspNetCore.Http;
//using System;
//using System.Threading.Tasks;

//namespace Scale.Fun.AspNetCore
//{
//    //TODO: Move to Fun<T, T>
//    public class HttpFunBinding<TInput, TOutput> : FunBinding<TInput, TOutput>
//    {
//        public HttpFunBinding(FunContext context) : base(context) { }

//        //public virtual async Task FunHttpRequestDelegate(HttpContext context)
//        //{
//        //    if (_fun is null) throw new InvalidOperationException("Fun has not been bound. Call Bind(fun).");

//        //    //TODO: 
//        //    //_fun.Authorize(context);

//        //    await context.Response.WriteAsJsonAsync(
//        //        await _fun(_context, await context.Request.ReadFromJsonAsync<TInput>()));
//        //}
//    }
//}
