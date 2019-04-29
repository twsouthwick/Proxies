using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<object> TranslateAsync(object obj, CancellationToken token)
        {
            if (obj is T t)
            {
                return await TranslateAsync(t, token);
            }
            else if (obj is IEnumerable<T> list)
            {
                return await TranslateAsync(list, token);
            }

            _logger.LogError("Unknown object {UnknownType}; Expected {Type}.", obj.GetType(), typeof(T));
            throw new InvalidOperationException("Must be of type T or IEnumerable<T>");
        }

        private Task<T[]> TranslateAsync(IEnumerable<T> list, CancellationToken token)
        {
            return Task.WhenAll(list.Select(async i =>
            {
                await TranslateAsync(i, token);
                return i;
            }));
        }

        private async Task<T> TranslateAsync(T instance, CancellationToken token)
        {
            foreach (var property in _translatable.Properties)
            {
                property.Setter(instance, await _translator.TranslateAsync(property.Getter(instance), token));
            }

            return instance;
        }
    }
}
