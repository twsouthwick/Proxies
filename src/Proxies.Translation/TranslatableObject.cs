using System.Linq;
using System.Reflection;

namespace Proxies.Translation
{
    internal class TranslatableObject<T>
        where T : class
    {
        public static TranslatableObject<T> Instance { get; } = new TranslatableObject<T>();

        public PropertyInfo<T>[] Properties { get; }

        private TranslatableObject()
        {
            Properties = typeof(T).GetProperties()
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<TranslateAttribute>() != null && propertyInfo.PropertyType == typeof(string))
                .Select(propertyInfo => new PropertyInfo<T>(propertyInfo))
                .ToArray();
        }
    }
}
