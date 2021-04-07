using Scale.Fun;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FunServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Fun integration to the provided <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <returns>The <see cref="IServiceCollection" /> service collection.</returns>
        //public static IServiceCollection AddFun<TInput, TOutput, TFun>(this IServiceCollection services) where TFun : class, IFun<TInput, TOutput>
        public static IServiceCollection AddFun(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // This pattern prevents registering services multiple times in the case AddFun is called
            // by non-user-code.
            if (!services.Any(s => s.ImplementationType == typeof(FunMarkerService)))
            {
                services.AddSingleton<ITelemetry, NullTelemetry>();
                services.AddSingleton<FunMarkerService>();
                services.AddSingleton<FunContext>();
                services.AddSingleton<FunController>();
            }

            return services;
        }

        /// <summary>
        /// Registers a type of <see cref="IFun"/> with the Service collection as a Transient. Also registers the core Fun services if they are not already.
        /// </summary>
        /// <typeparam name="T">Any type that implements <see cref="IFun"/></typeparam>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <returns>The <see cref="IServiceCollection" /> service collection.</returns>
        /// <remarks>This is convenient helper and only needs to be called if you are not registering IFun using a different method.</remarks>
        public static IServiceCollection AddFun<T>(this IServiceCollection services) where T : class, IFun
        {
            AddFun(services);

            if (!services.Any(s => s.ImplementationType == typeof(T))) services.AddTransient<T>();

            return services;
        }

        private class FunMarkerService
        {
        }
    }
}
