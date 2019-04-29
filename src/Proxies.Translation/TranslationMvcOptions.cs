using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Proxies.Translation
{
    internal class TranslationMvcOptions : IPostConfigureOptions<MvcOptions>
    {
        private readonly TranslationFilter _filter;

        public TranslationMvcOptions(TranslationFilter filter)
        {
            _filter = filter;
        }

        public void PostConfigure(string name, MvcOptions options)
        {
            options.Filters.Insert(options.Filters.Count, _filter);
        }
    }
}
