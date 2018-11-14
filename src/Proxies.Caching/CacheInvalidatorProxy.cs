using Castle.DynamicProxy;

namespace Proxies.Caching
{
    internal class CacheInvalidatorProxy<T> : ICacheInvalidatorProxy<T>
        where T : class
    {
        public CacheInvalidatorProxy(
            ProxyGenerator generator,
            CacheInvalidatorInterceptor interceptor,
            CachedProxyGenerationOptions options)
        {
            var throwing = generator.CreateInterfaceProxyWithoutTarget<T>(CacheProxyExceptionInterceptor.Instance);

            Value = generator.CreateInterfaceProxyWithTargetInterface<T>(throwing, options, interceptor);
        }

        public T Value { get; }
    }
}
