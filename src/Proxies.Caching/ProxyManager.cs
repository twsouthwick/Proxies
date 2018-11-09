using Castle.DynamicProxy;

namespace Proxies.Caching
{
    internal class ProxyManager
    {
        private readonly CachingInterceptor _cachingInterceptor;
        private readonly CacheInvalidatorInterceptor _invalidatorInterceptor;
        private readonly ProxyGenerator _generator;
        private readonly ProxyGenerationOptions _options;

        public ProxyManager(
           CachingInterceptor interceptor,
           CacheInvalidatorInterceptor invalidatorInterceptor,
           ReadonlyProxyGenerationHook hook)
        {
            _cachingInterceptor = interceptor;
            _invalidatorInterceptor = invalidatorInterceptor;
            _generator = new ProxyGenerator();
            _options = new ProxyGenerationOptions(hook);
        }

        public T CreateCachingProxy<T>(T target)
            where T : class
        {
            return _generator.CreateInterfaceProxyWithTargetInterface(target, _options, _cachingInterceptor.ToInterceptor());
        }

        public T CreateInvalidatorProxy<T>()
            where T : class
        {
            return _generator.CreateInterfaceProxyWithoutTarget<T>(_options, _invalidatorInterceptor.ToInterceptor());
        }
    }
}
