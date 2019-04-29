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

        public Task TranslateAsync(object obj, string language)
        {
            if (obj is T t)
            {
                return _factory.TranslateAsync((T)obj, language);
            }
            else if (obj is IEnumerable<T> list)
            {
                return Task.WhenAll(list.Select(i => _factory.TranslateAsync(i, language)));
            }

            throw new InvalidOperationException("Must be of type T or IEnumerable<T>");
        }
    }
}
