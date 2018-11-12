namespace Proxies.Caching
{
    internal static class SR
    {
        public static string Format(string message, params object[] args)
        {
            return string.Format(LocalizedStrings.Culture, message, args); ;
        }
    }
}
