using System.Threading.Tasks;

namespace Proxies.Translation
{
    public interface ITranslatorFactory<T>
        where T : class
    {
        Task TranslateAsync(T instance, string language);
    }
}
