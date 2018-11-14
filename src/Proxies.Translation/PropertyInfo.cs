using System;
using System.Reflection;

namespace Proxies.Translation
{
    internal readonly struct PropertyInfo<T>
        where T : class
    {
        public PropertyInfo(PropertyInfo property)
        {
            Getter = property.GetStringValueGetter<T>();
            Setter = property.GetStringValueSetter<T>();
        }

        public Func<T, string> Getter { get; }

        public Action<T, string> Setter { get; }
    }
}
