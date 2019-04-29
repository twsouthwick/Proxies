using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Proxies.Translation
{
    public static class DependencyExtensions
    {
        public static void AddTranslation(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ObjectTranslator<>));
            services.AddTransient<IPostConfigureOptions<MvcOptions>, TranslationMvcOptions>();
            services.AddSingleton<TranslationResultFilter>();
            services.AddSingleton<ITranslator, EmptyTranslator>();
        }
    }
}
