using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Proxies
{
    public abstract class TestBase : IClassFixture<TestScopeProvider>, IDisposable
    {
        private readonly ILifetimeScope _scope;

        protected virtual Fixture Fixture { get; }

        public TestBase(TestScopeProvider scopeProvider)
        {
            _scope = scopeProvider.Container.BeginLifetimeScope(AddServices);
            Fixture = new Fixture();

            Customize(Fixture);
        }

        private void AddServices(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            CustomizeServices(services);

            builder.Populate(services);
        }

        protected virtual void Customize(Fixture fixture)
        {
        }

        protected virtual void CustomizeServices(IServiceCollection services)
        {
        }

        protected T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
