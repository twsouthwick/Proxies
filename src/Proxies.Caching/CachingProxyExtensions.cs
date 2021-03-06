﻿using Castle.DynamicProxy;
using Proxies.Caching;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CachingProxyExtensions
    {
        public static void AddCaching(this IServiceCollection services)
        {
            services.AddSingleton(_ => new ProxyGenerator());
            services.AddSingleton<CachedProxyGenerationOptions>();
            services.AddSingleton<CachedProxyGenerationHook>();

            services.AddSingleton<CachingInterceptor>();
            services.AddSingleton<CacheInvalidatorInterceptor>();
            services.AddSingleton<CacheProxyExceptionInterceptor>();

            services.AddSingleton<IKeyGenerator, KeyGenerator>();

            services.AddSingleton(typeof(ICachedProxy<>), typeof(CachedProxy<>));
            services.AddSingleton(typeof(ICacheInvalidatorProxy<>), typeof(CacheInvalidatorProxy<>));
        }
    }
}
