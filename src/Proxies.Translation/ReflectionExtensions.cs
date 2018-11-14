using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Proxies.Translation
{
    internal static class ReflectionExtensions
    {
        // From example at https://weblogs.asp.net/marianor/using-expression-trees-to-get-property-getter-and-setters
        public static Func<T, string> GetStringValueGetter<T>(this PropertyInfo propertyInfo)
            where T : class
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (typeof(T) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException("Mismatch between type argument and property's declaring type");
            }

            if (typeof(string) != propertyInfo.PropertyType)
            {
                throw new ArgumentException("Expecting string property but get {0}", propertyInfo.PropertyType.ToString());
            }

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var property = Expression.Property(instance, propertyInfo);

            return (Func<T, string>)Expression.Lambda(property, instance).Compile();
        }

        public static Action<T, string> GetStringValueSetter<T>(this PropertyInfo propertyInfo)
            where T : class
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (typeof(T) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException("Mismatch between type argument and property's declaring type");
            }

            if (typeof(string) != propertyInfo.PropertyType)
            {
                throw new ArgumentException("Expecting string property but get {0}", propertyInfo.PropertyType.ToString());
            }

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var argument = Expression.Parameter(typeof(string), "a");
            var setterCall = Expression.Call(
                instance,
                propertyInfo.GetSetMethod(),
                argument);

            return (Action<T, string>)Expression.Lambda(setterCall, instance, argument).Compile();
        }
    }
}
