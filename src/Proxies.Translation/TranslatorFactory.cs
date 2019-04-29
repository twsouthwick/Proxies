using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal class TranslatorFactory<T> : ITranslatorFactory<T>
        where T : class
    {
        private readonly TranslatableObject<T> _translatable;
        private readonly ITranslator _translator;

        public TranslatorFactory(ITranslator translator)
        {
            _translator = translator;
            _translatable = new TranslatableObject<T>();
        }

        public bool IsEmpty => _translatable.Properties.Length == 0;

        public async Task TranslateAsync(T instance)
        {
            foreach (var property in _translatable.Properties)
            {
                property.Setter(instance, await _translator.TranslateAsync(property.Getter(instance)));
            }
        }
    }
}
