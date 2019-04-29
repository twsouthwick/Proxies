using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Proxies.Translation
{
    public static class DependencyExtensions
    {
        public static void AddTranslation(this IServiceCollection services)
        {
            // Instance of ObjectTranslator<> are retrieved and cached within the filter, so no need for the DI system to cache.
            services.AddTransient(typeof(ObjectTranslator<>));

            // The configuration only occurs once, so no need to have the DI system cache the configuration or the filter.
            services.AddTransient<IPostConfigureOptions<MvcOptions>, TranslationMvcOptions>();
            services.AddTransient<TranslationResultFilter>();

            services.AddSingleton<ITranslator, EmptyTranslator>();
        }
    }
}
