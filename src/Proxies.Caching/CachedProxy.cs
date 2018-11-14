using Castle.DynamicProxy;

namespace Proxies.Caching
{
    internal class CachedProxy<T> : ICachedProxy<T>
        where T : class
    {
        public CachedProxy(
            T target,
            ProxyGenerator generator,
            CachingInterceptor interceptor,
            CachedProxyGenerationOptions options)
        {
            Value = generator.CreateInterfaceProxyWithTargetInterface(target, options, interceptor);
        }

        public T Value { get; }
    }
}
