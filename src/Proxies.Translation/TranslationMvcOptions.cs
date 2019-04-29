using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Proxies.Translation
{
    internal class TranslationMvcOptions : IPostConfigureOptions<MvcOptions>
    {
        private readonly TranslationResultFilter _filter;

        public TranslationMvcOptions(TranslationResultFilter filter)
        {
            _filter = filter;
        }

        public void PostConfigure(string name, MvcOptions options)
        {
            options.Filters.Insert(0, _filter);
        }
    }
}
