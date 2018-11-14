namespace Proxies.Translation
{
    public interface ITranslatorFactory<T>
        where T : class
    {
        T CreateTranslator(string language);
    }
}
