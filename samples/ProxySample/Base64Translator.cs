using System;
using System.Text;
using System.Threading.Tasks;
using Proxies.Translation;

namespace ProxySample
{
    internal class Base64Translator : ITranslator
    {
        public Task<string> TranslateAsync(string input, string language)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var converted = Convert.ToBase64String(bytes);

            return Task.FromResult(converted);
        }
    }
}
