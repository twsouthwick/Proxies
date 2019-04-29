using Microsoft.Extensions.DependencyInjection;

namespace Proxies.Translation
{
    public class TranslationBuilder
    {
        public TranslationBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public TranslationBuilder AddTranslator<T>()
            where T : class, ITranslator
        {
            Services.AddSingleton<ITranslator, T>();

            return this;
        }
    }
}
