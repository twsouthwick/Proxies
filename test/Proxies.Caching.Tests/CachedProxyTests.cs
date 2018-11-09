using Microsoft.Extensions.DependencyInjection;
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
        public async Task SimpleCaching()
        {
            var cached = Resolve<ICachedProxy<IA>>();

            string result = await cached.Value.GetAsync();
        }

        public interface IA
        {
            [Cached]
            Task<string> GetAsync();
        }
    }
}
