using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal interface IObjectTranslator
    {
        Task TranslateAsync(object obj, string language);
    }
}
