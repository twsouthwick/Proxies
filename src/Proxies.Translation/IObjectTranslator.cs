using System.Threading;
using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal interface IObjectTranslator
    {
        Task<object> TranslateAsync(object obj, CancellationToken token);

        int Count { get; }
    }
}
