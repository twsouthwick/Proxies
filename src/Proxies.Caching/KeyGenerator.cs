using System.Reflection;
using System.Text;

namespace Proxies.Caching
{
    internal class KeyGenerator : IKeyGenerator
    {
        public string GenerateKey(MethodInfo method, params object[] args)
        {
            var sb = new StringBuilder();

            sb.Append(method.DeclaringType.FullName);
            sb.Append('.');
            sb.Append(method.DeclaringType.GetType().FullName);
            sb.Append('.');
            sb.Append(method.Name);
            sb.Append('.');

            foreach (object a in args)
            {
                sb.Append(a);
                sb.Append(',');
            }

            return sb.ToString();
        }
    }
}
