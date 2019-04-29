using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal interface IObjectTranslator
    {
        Task<object> TranslateAsync(object obj, string language);
    }
}
