using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal interface ITranslatorFactory<T>
        where T : class
    {
        Task TranslateAsync(T instance, string language);

        bool IsEmpty { get; }
    }
}
