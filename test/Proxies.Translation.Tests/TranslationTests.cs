using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Proxies.Translation.Tests
{
    public class TranslationTests : TestBase
    {
        public TranslationTests(TestScopeProvider scopeProvider)
            : base(scopeProvider)
        {
        }

        protected override void CustomizeServices(IServiceCollection services)
        {
            services.AddTranslation();
        }

        //[Fact]
        //public void InvalidOperationThrown()
        //{
        //    Assert.Throws<InvalidOperationException>(() => Resolve<ITranslatorFactory<IA>>().CreateTranslator(Fixture.Create<string>()));
        //}

        //[Fact]
        //public void ArgumentNullCheck()
        //{
        //    Assert.Throws<ArgumentNullException>(() => Resolve<ITranslatorFactory<IA>>().CreateTranslator(null));
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData("  ")]
        //public void ArgumentOutOfRange(string language)
        //{
        //    Assert.Throws<ArgumentOutOfRangeException>(() => Resolve<ITranslatorFactory<IA>>().CreateTranslator(language));
        //}

        public interface IA
        {
        }
    }
}
