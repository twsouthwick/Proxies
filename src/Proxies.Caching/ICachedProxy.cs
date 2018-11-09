namespace Proxies.Caching
{
    public interface ICachedProxy<T>
        where T : class
    {
        T Value { get; }
    }
}
