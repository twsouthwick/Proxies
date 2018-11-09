namespace Proxies.Caching
{
    public interface ICacheInvalidatorProxy<T>
        where T : class
    {
        T Value { get; }
    }
}
