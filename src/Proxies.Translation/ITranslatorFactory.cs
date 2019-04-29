using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal interface ITranslatorFactory<T>
        where T : class
    {
        Task TranslateAsync(T instance);

        bool IsEmpty { get; }
    }
}
