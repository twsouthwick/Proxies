using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Proxies.Caching
{
    internal class CacheInvalidatorInterceptor : AsyncInterceptorBase
    {
        protected override Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            return Task.CompletedTask;
        }

        protected override Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            return Task.FromResult<TResult>(default);
        }
    }
}
