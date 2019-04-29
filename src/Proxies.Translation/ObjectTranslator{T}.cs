using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ObjectTranslator<T>> _logger;

        public ObjectTranslator(ITranslator translator, ILogger<ObjectTranslator<T>> logger)
        {
            _translator = translator;
            _logger = logger;
            _translatable = new TranslatableObject<T>();
        }

        public int Count => _translatable.Properties.Length;

        public async Task<object> TranslateAsync(object obj)
        {
            if (obj is T t)
            {
                return await TranslateAsync(t);
            }
            else if (obj is IEnumerable<T> list)
            {
                return await TranslateAsync(list);
            }

            _logger.LogError("Unknown object {UnknownType}; Expected {Type}.", obj.GetType(), typeof(T));
            throw new InvalidOperationException("Must be of type T or IEnumerable<T>");
        }

        private Task<T[]> TranslateAsync(IEnumerable<T> list)
        {
            return Task.WhenAll(list.Select(async i =>
            {
                await TranslateAsync(i);
                return i;
            }));
        }

        private async Task<T> TranslateAsync(T instance)
        {
            foreach (var property in _translatable.Properties)
            {
                property.Setter(instance, await _translator.TranslateAsync(property.Getter(instance)));
            }

            return instance;
        }
    }
}
