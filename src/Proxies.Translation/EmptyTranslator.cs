using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal class EmptyTranslator : ITranslator
    {
        public Task<string> TranslateAsync(string input, string language) => Task.FromResult(input);
    }
}
