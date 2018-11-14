using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
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
            // Arrange
            var invalidator = Resolve<ICacheInvalidatorProxy<IA>>();
            string expected = Fixture.Create<string>();
            string key = Resolve<IKeyGenerator>().GenerateKey(typeof(IA).GetMethod(nameof(IA.GetAsync)));

            Resolve<IA>().GetAsync().Returns(expected);

            // Act
            string result = await invalidator.Value.GetAsync();

            // Assert
            Assert.Null(result);

            await Resolve<IDistributedCache>().Received(1).RemoveAsync(key);
        }

        [Fact]
        public async Task InvalidationWithYield()
        {
            // Arrange
            var invalidator = Resolve<ICacheInvalidatorProxy<IA>>();
            string expected = Fixture.Create<string>();
            string key = Resolve<IKeyGenerator>().GenerateKey(typeof(IA).GetMethod(nameof(IA.GetAsync)));

            async Task<string> YieldAsync()
            {
                await Task.Yield();
                return expected;
            }

            Resolve<IA>().GetAsync().Returns(_ => YieldAsync());

            // Act
            string result = await invalidator.Value.GetAsync();

            // Assert
            Assert.Null(result);

            await Resolve<IDistributedCache>().Received(1).RemoveAsync(key);
        }

        [Fact]
        public async Task InvalidationWithDelay()
        {
            // Arrange
            var invalidator = Resolve<ICacheInvalidatorProxy<IA>>();
            string expected = Fixture.Create<string>();
            string key = Resolve<IKeyGenerator>().GenerateKey(typeof(IA).GetMethod(nameof(IA.GetAsync)));

            async Task<string> DelayAsync()
            {
                await Task.Delay(100);
                return expected;
            }

            Resolve<IA>().GetAsync().Returns(_ => DelayAsync());

            // Act
            string result = await invalidator.Value.GetAsync();

            // Assert
            Assert.Null(result);

            await Resolve<IDistributedCache>().Received(1).RemoveAsync(key);
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
