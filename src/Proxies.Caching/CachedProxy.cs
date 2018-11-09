namespace Proxies.Caching
{
    internal class CachedProxy<T> : ICachedProxy<T>
        where T : class
    {
        public CachedProxy(T target, ProxyManager manager)
        {
            Value = manager.CreateCachingProxy<T>(target);
        }

        public T Value { get; }
    }
}
