using System.Reflection;

namespace Proxies.Caching
{
    public interface IKeyGenerator
    {
        string GenerateKey(MethodInfo method, params object[] args);
    }
}
