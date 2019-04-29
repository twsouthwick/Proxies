using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Proxies.Translation;

namespace ProxySample
{
    internal class Base64Translator : ITranslator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Base64Translator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> TranslateAsync(string input)
        {
            // Potentially grab information from the HTTP context or some other service to determine what kind of translation should be done
            var bytes = Encoding.UTF8.GetBytes(input);
            var converted = Convert.ToBase64String(bytes);

            return Task.FromResult(converted);
        }
    }
}
