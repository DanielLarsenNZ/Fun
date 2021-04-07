using Fun;
using Fun.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class HttpFunApplicationBuilderExtensions
    {
        //public static IApplicationBuilder UseFun<TBinding, TFun>(this IApplicationBuilder app, string route) where TBinding : IFunBinding where TFun : IFun
        //{
        //    var fun = app.ApplicationServices.GetService<TFun>();
        //    var binding = app.ApplicationServices.GetService<TBinding>();

        //    if (fun is null)
        //    {
        //        throw new NullReferenceException($"A service named \"{typeof(T).Name}\" cannot be found in the Application Services. Ensure AddFun<{typeof(T).Name}>() is called in ConfigureServices().");
        //    }

        //    binding.Bind();
        //    fun.Bind();


        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapPost(route, fun.RequestDelegate);
        //    });

        //    return app;
        //}

        /// <summary>
        /// Binds an instance of IFun and Maps to a POST HttpRequest
        /// </summary>
        /// <typeparam name="T">A registered type of <see cref="IFun"/></typeparam>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="route">Route pattern for the POST request</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseFunPost<T>(this IApplicationBuilder app, string route) where T : IHttpFunBinding
        {
            var fun = GetAndBind<T>(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost(route, fun.RequestDelegate);
            });

            return app;
        }

        //TODO: Query string binding
        ///// <summary>
        ///// Binds an instance of IFun and Maps to a GET HttpRequest
        ///// </summary>
        ///// <typeparam name="T">A registered type of <see cref="IFun"/></typeparam>
        ///// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        ///// <param name="route">Route pattern for the POST request</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        //public static IApplicationBuilder UseFunGet<T>(this IApplicationBuilder app, string route) where T : IHttpFun
        //{
        //    var fun = GetAndBind(app);

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapGet(route, fun.RequestDelegate);
        //    });

        //    return app;
        //}

        private static IHttpFunBinding GetAndBind<T>(IApplicationBuilder app) where T: IHttpFunBinding
        {
            var fun = app.ApplicationServices.GetService<T>();

            if (fun is null)
            {
                throw new NullReferenceException($"A service named \"{typeof(T).Name}\" cannot be found in the Application Services. Ensure AddFun<{typeof(T).Name}>() is called in ConfigureServices().");
            }

            fun.Bind();

            return fun;
        }
    }
}
