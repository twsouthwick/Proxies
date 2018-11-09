namespace Proxies.Caching
{
    internal class CacheInvalidatorProxy<T> : ICacheInvalidatorProxy<T>
        where T : class
    {
        public CacheInvalidatorProxy(ProxyManager manager)
        {
            Value = manager.CreateInvalidatorProxy<T>();
        }

        public T Value { get; }
    }
}
