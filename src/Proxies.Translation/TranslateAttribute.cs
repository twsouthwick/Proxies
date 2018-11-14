using System;

namespace Proxies.Translation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class TranslateAttribute : Attribute
    {
    }
}
