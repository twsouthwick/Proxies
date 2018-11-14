using Castle.DynamicProxy;

namespace Proxies.Caching
{
    internal class CacheInvalidatorProxy<T> : ICacheInvalidatorProxy<T>
        where T : class
    {
        public CacheInvalidatorProxy(
            ProxyGenerator generator,
            CacheInvalidatorInterceptor invalidatorInterceptor,
            CacheProxyExceptionInterceptor exceptionInterceptor,
            CachedProxyGenerationOptions options)
        {
            var throwing = generator.CreateInterfaceProxyWithoutTarget<T>(exceptionInterceptor);

            Value = generator.CreateInterfaceProxyWithTargetInterface(throwing, options, invalidatorInterceptor);
        }

        public T Value { get; }
    }
}
