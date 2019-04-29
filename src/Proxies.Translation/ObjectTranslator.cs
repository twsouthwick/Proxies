using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal class ObjectTranslator<T> : IObjectTranslator
        where T : class
    {
        private readonly ITranslatorFactory<T> _factory;

        public ObjectTranslator(ITranslatorFactory<T> factory)
        {
            _factory = factory;
        }

        public bool IsEmpty => _factory.IsEmpty;

        public async Task<object> TranslateAsync(object obj, string language)
        {
            if (obj is T t)
            {
                await _factory.TranslateAsync((T)obj, language);
                return t;
            }
            else if (obj is IEnumerable<T> list)
            {
                return await Task.WhenAll(list.Select(async i =>
                {
                    await _factory.TranslateAsync(i, language);
                    return i;
                }));
            }

            throw new InvalidOperationException("Must be of type T or IEnumerable<T>");
        }
    }
}
