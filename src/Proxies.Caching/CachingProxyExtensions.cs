using Microsoft.Extensions.DependencyInjection;

namespace Proxies.Caching
{
    public static class CachingProxyExtensions
    {
        public static void AddCaching(this IServiceCollection services)
        {
            services.AddSingleton<ProxyManager>();
            services.AddSingleton<ReadonlyProxyGenerationHook>();

            services.AddSingleton<CachingInterceptor>();
            services.AddSingleton<CacheInvalidatorInterceptor>();

            services.AddSingleton<IKeyGenerator, KeyGenerator>();

            services.AddSingleton(typeof(ICachedProxy<>), typeof(CachedProxy<>));
            services.AddSingleton(typeof(ICacheInvalidatorProxy<>), typeof(CacheInvalidatorProxy<>));
        }
    }
}
