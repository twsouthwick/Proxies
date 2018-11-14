using Castle.DynamicProxy;

namespace Proxies.Caching
{
    internal class CachedProxyGenerationOptions : ProxyGenerationOptions
    {
        public CachedProxyGenerationOptions(CachedProxyGenerationHook hook)
            : base(hook)
        {
        }
    }
}
