using Fun.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Scale.Fun;
using System;

namespace Microsoft.AspNetCore.Builder
{ 
    public static class HttpFunApplicationBuilderExtensions
    {
        /// <summary>
        /// Binds an instance of IFun and Maps to a POST HttpRequest
        /// </summary>
        /// <typeparam name="T">A regsitered type of <see cref="IFun"/></typeparam>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="route">Route pattern for the POST request</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseFun<T>(this IApplicationBuilder app, string route) where T : IHttpFun
        {
            var fun = app.ApplicationServices.GetService<T>();

            if (fun is null)
            {
                throw new NullReferenceException($"A service named \"{typeof(T).Name}\" cannot be found in the Application Services. Ensure AddFun<{typeof(T).Name}>() is called in ConfigureServices().");
            }

            fun.Bind();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost(route, fun.RequestDelegate);
            });

            return app;
        }
    }
}
