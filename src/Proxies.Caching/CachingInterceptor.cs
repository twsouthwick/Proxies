using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Proxies.Caching
{
    internal class CachingInterceptor : AsyncInterceptorBase
    {
        private readonly IKeyGenerator _keyGenerator;
        private readonly IDistributedCache _cache;
        private readonly ICacheSerializer _serializer;
        private readonly ILogger<CachingInterceptor> _logger;

        public CachingInterceptor(
            IKeyGenerator keyGenerator,
            IDistributedCache cache,
            ICacheSerializer serializer,
            ILogger<CachingInterceptor> logger)
        {
            _keyGenerator = keyGenerator;
            _cache = cache;
            _serializer = serializer;
            _logger = logger;
        }

        protected override Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            return proceed(invocation);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            string key = _keyGenerator.GenerateKey(invocation.Method, invocation.Arguments);

            byte[] cached = await _cache.GetAsync(key);

            if (cached is null || cached.Length == 0)
            {
                _logger.LogTrace("Cached value unavailable for {Method}", invocation.Method);

                var value = await proceed(invocation);

                _logger.LogTrace("Setting cached value for {Method}", invocation.Method);
                await _cache.SetAsync(key, _serializer.Serialize(value));

                return value;
            }
            else
            {
                _logger.LogTrace("Using cached value for {Method}", invocation.Method);

                return _serializer.Deserialize<TResult>(cached);
            }
        }
    }
}
