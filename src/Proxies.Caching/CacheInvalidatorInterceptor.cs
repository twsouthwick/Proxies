using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Proxies.Caching
{
    internal class CacheInvalidatorInterceptor : AsyncInterceptorBase
    {
        private readonly IKeyGenerator _keyGenerator;
        private readonly IDistributedCache _cache;

        public CacheInvalidatorInterceptor(
            IKeyGenerator keyGenerator,
            IDistributedCache cache)
        {
            _keyGenerator = keyGenerator;
            _cache = cache;
        }

        protected override Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            return Task.CompletedTask;
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            string key = _keyGenerator.GenerateKey(invocation.Method, invocation.Arguments);

            await _cache.RemoveAsync(key);

            return default;
        }
    }
}
