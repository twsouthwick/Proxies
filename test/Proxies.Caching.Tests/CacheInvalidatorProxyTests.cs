using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Proxies.Caching.Tests
{
    public class CacheInvalidatorProxyTests : TestBase
    {
        public CacheInvalidatorProxyTests(TestScopeProvider scopeProvider)
            : base(scopeProvider)
        {
        }

        protected override void CustomizeServices(IServiceCollection services)
        {
            services.AddCaching();
        }

        [Fact]
        public void IsProxied()
        {
            var original = Resolve<IA>();
            var cached = Resolve<ICacheInvalidatorProxy<IA>>().Value;

            Assert.NotSame(original, cached);
        }

        [Fact]
        public void SameInstance()
        {
            var cached1 = Resolve<ICacheInvalidatorProxy<IA>>();
            var cached2 = Resolve<ICacheInvalidatorProxy<IA>>();

            Assert.Same(cached1, cached2);
        }

        [Fact]
        public async Task SimpleInvalidation()
        {
            var cached = Resolve<ICacheInvalidatorProxy<IA>>();

            string result = await cached.Value.GetAsync();
        }

        [Fact]
        public async Task InvalidationOnNonCacheableMethodThrows()
        {
            var invalidator = Resolve<ICacheInvalidatorProxy<IB>>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => invalidator.Value.NotCachedAsync());
        }

        public interface IA
        {
            [Cached]
            Task<string> GetAsync();
        }

        public interface IB
        {
            Task<string> NotCachedAsync();
        }
    }
}
