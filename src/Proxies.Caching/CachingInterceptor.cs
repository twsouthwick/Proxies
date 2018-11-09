using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Proxies.Caching
{
    internal class CachingInterceptor : AsyncInterceptorBase
    {
        protected override Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            return proceed(invocation);
        }

        protected override Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            return proceed(invocation);
        }
    }
}
