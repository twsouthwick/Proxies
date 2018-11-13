using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Proxies.Caching
{
    internal abstract class AsyncInterceptor : IInterceptor
    {
        private static readonly MethodInfo _handleAsyncWithResult = typeof(AsyncInterceptor).GetMethod(nameof(AsyncInterceptor.HandleAsyncWithResult), BindingFlags.Instance | BindingFlags.NonPublic);

        public void Intercept(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultType = returnType.GetGenericArguments()[0];
                var mi = _handleAsyncWithResult.MakeGenericMethod(resultType);
                invocation.ReturnValue = mi.Invoke(this, new[] { invocation });
            }
            else
            {
                throw new InvalidOperationException(LocalizedStrings.OnlyTaskT);
            }
        }

        protected abstract Task<T> InterceptAsync<T>(IInvocation invocation, Func<Task<T>> proceed);

        private Task<T> HandleAsyncWithResult<T>(IInvocation invocation)
        {
            return InterceptAsync(invocation, () =>
            {
                invocation.Proceed();
                return (Task<T>)invocation.ReturnValue;
            });
        }
    }
}
