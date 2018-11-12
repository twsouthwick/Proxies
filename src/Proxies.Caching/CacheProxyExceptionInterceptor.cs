using Castle.DynamicProxy;
using System;

namespace Proxies.Caching
{
    internal class CacheProxyExceptionInterceptor : IInterceptor
    {
        public static IInterceptor Instance { get; } = new CacheProxyExceptionInterceptor();

        private CacheProxyExceptionInterceptor()
        {
        }

        public void Intercept(IInvocation invocation)
        {
            throw new InvalidOperationException(SR.Format(LocalizedStrings.MethodNotCached, invocation.Method.Name));
        }
    }
}
