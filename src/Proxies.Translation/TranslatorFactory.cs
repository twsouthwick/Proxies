using System;

namespace Proxies.Translation
{
    internal class TranslatorFactory<T> : ITranslatorFactory<T>
        where T : class
    {
        public T CreateTranslator(string language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentOutOfRangeException(nameof(language));
            }

            throw new InvalidOperationException();
        }
    }
}
