using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal class ObjectTranslator<T> : IObjectTranslator
        where T : class
    {
        private readonly TranslatableObject<T> _translatable;
        private readonly ITranslator _translator;

        public ObjectTranslator(ITranslator translator)
        {
            _translator = translator;
            _translatable = new TranslatableObject<T>();
        }

        public bool IsEmpty => _translatable.Properties.Length == 0;

        public async Task<object> TranslateAsync(object obj)
        {
            if (obj is T t)
            {
                await TranslateAsync((T)obj);
                return t;
            }
            else if (obj is IEnumerable<T> list)
            {
                return await Task.WhenAll(list.Select(async i =>
                {
                    await TranslateAsync(i);
                    return i;
                }));
            }

            throw new InvalidOperationException("Must be of type T or IEnumerable<T>");
        }

        private async Task TranslateAsync(T instance)
        {
            foreach (var property in _translatable.Properties)
            {
                property.Setter(instance, await _translator.TranslateAsync(property.Getter(instance)));
            }
        }
    }
}
