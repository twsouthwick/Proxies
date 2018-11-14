using Castle.DynamicProxy;
using System;

namespace Proxies.Caching
{
    internal class CacheProxyExceptionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new InvalidOperationException(SR.Format(LocalizedStrings.MethodNotCached, invocation.Method.Name));
        }
    }
}
