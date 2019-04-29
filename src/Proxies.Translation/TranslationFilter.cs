using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Proxies.Translation
{
    internal class TranslationFilter : IAsyncResultFilter
    {
        private readonly ConcurrentDictionary<Type, IObjectTranslator> _translators = new ConcurrentDictionary<Type, IObjectTranslator>();

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (IsSuccess(context.HttpContext.Response.StatusCode))
            {
                await UpdateValue(context.Result, context.HttpContext.RequestServices);
            }

            await next();
        }

        private async Task UpdateValue(IActionResult result, IServiceProvider services)
        {
            if (result is ObjectResult objResult && _translators.GetOrAdd(objResult.DeclaredType, CreateTranslator, services) is IObjectTranslator translator)
            {
                objResult.Value = await translator.TranslateAsync(objResult.Value).ConfigureAwait(false);
            }
        }

        private IObjectTranslator CreateTranslator(Type type, IServiceProvider services)
        {
            var expectedType = typeof(ObjectTranslator<>).MakeGenericType(UnwrapType(type));
            var translator = (IObjectTranslator)services.GetService(expectedType);

            return translator.IsEmpty ? null : translator;
        }

        private Type UnwrapType(Type declared)
        {
            if (declared.IsGenericType && declared.GenericTypeArguments.Length == 1 && declared.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return declared.GenericTypeArguments[0];
            }

            return declared;
        }

        private static bool IsSuccess(int statusCode) => statusCode >= 200 && statusCode < 300;
    }
}
