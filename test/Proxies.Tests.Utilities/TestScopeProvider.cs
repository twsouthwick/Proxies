using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.ResolveAnything;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proxies
{
    public sealed class TestScopeProvider : IDisposable
    {
        public TestScopeProvider()
        {
            var builder = new ContainerBuilder();

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.RegisterSource(new NSubstituteRegistrationSource());

            Container = builder.Build();
        }

        public void Dispose()
        {
            Container.Dispose();
        }

        internal IContainer Container { get; }

        private class NSubstituteRegistrationSource : IRegistrationSource
        {
            public bool IsAdapterForIndividualComponents => false;

            public IEnumerable<IComponentRegistration> RegistrationsFor(Autofac.Core.Service service, Func<Autofac.Core.Service, IEnumerable<IComponentRegistration>> registrationAccessor)
            {
                if (service is IServiceWithType ts && (ts.ServiceType.IsInterface || ts.ServiceType.IsAbstract))
                {
                    var builder = RegistrationBuilder.ForDelegate((c, p) => Substitute.For(new[] { ts.ServiceType }, null))
                        .As(service)
                        .InstancePerLifetimeScope();

                    return new[] { builder.CreateRegistration() };
                }

                return Enumerable.Empty<IComponentRegistration>();
            }
        }
    }
}
