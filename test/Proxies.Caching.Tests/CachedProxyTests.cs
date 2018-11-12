using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Proxies.Caching.Tests
{
    public class CachedProxyTests : TestBase
    {
        public CachedProxyTests(TestScopeProvider scopeProvider)
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
            var cached = Resolve<ICachedProxy<IA>>().Value;

            Assert.NotSame(original, cached);
        }

        [Fact]
        public void SameInstance()
        {
            var cached1 = Resolve<ICachedProxy<IA>>();
            var cached2 = Resolve<ICachedProxy<IA>>();

            Assert.Same(cached1, cached2);
        }

        [Fact]
        public async Task ProxiesToTarget()
        {
            // Arrange
            var cached = Resolve<ICachedProxy<IA>>();
            string expected = Fixture.Create<string>();

            Resolve<IA>().GetAsync().Returns(expected);

            // Act
            string result = await cached.Value.GetAsync();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task SimpleCaching()
        {
            // Arrange
            var cached = Resolve<ICachedProxy<IA>>();
            var cache = Resolve<IDistributedCache>();
            string expected = Fixture.Create<string>();
            string key = Resolve<IKeyGenerator>().GenerateKey(typeof(IA).GetMethod(nameof(IA.GetAsync)));
            byte[] bytes = Fixture.CreateMany<byte>().ToArray();

            Resolve<IA>().GetAsync().Returns(expected);
            Resolve<ICacheSerializer>().Serialize(expected).Returns(bytes);

            // Act
            string result = await cached.Value.GetAsync();

            // Assert
            Assert.Equal(expected, result);

            await Resolve<IA>().Received(1).GetAsync();
            await Resolve<IDistributedCache>().Received(1).GetAsync(key);
            await Resolve<IDistributedCache>().Received(1).SetAsync(key, bytes, Arg.Any<DistributedCacheEntryOptions>());
        }

        [Fact]
        public async Task CachedValueAvailable()
        {
            // Arrange
            var cached = Resolve<ICachedProxy<IA>>();
            var cache = Resolve<IDistributedCache>();
            string expected = Fixture.Create<string>();
            string key = Resolve<IKeyGenerator>().GenerateKey(typeof(IA).GetMethod(nameof(IA.GetAsync)));
            byte[] bytes = Fixture.CreateMany<byte>().ToArray();

            Resolve<IDistributedCache>().GetAsync(key).Returns(bytes);
            Resolve<ICacheSerializer>().Deserialize<string>(bytes).Returns(expected);

            // Act
            string result = await cached.Value.GetAsync();

            // Assert
            Assert.Equal(expected, result);

            await Resolve<IA>().Received(0).GetAsync();
            await Resolve<IDistributedCache>().Received(1).GetAsync(key);
            await Resolve<IDistributedCache>().Received(0).SetAsync(key, bytes, Arg.Any<DistributedCacheEntryOptions>());
        }

        public interface IA
        {
            [Cached]
            Task<string> GetAsync();
        }
    }
}
