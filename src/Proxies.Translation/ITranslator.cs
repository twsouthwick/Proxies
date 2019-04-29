using System.Threading.Tasks;

namespace Proxies.Translation
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string input);
    }
}
