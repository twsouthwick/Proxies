namespace Proxies.Caching
{
    public interface ICacheSerializer
    {
        byte[] Serialize<T>(T item);

        T Deserialize<T>(byte[] bytes);
    }
}
