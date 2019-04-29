using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Proxies.Translation
{
    internal class TranslationFilter : IAsyncResultFilter
    {
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
            if (result is ObjectResult objResult)
            {
                var expectedType = typeof(ObjectTranslator<>).MakeGenericType(UnwrapType(objResult));
                var translator = (IObjectTranslator)services.GetService(expectedType);

                objResult.Value = await translator.TranslateAsync(objResult.Value, string.Empty).ConfigureAwait(false);
            }
        }

        private Type UnwrapType(ObjectResult result)
        {
            var declared = result.DeclaredType;

            if (declared.IsGenericType && declared.GenericTypeArguments.Length == 1 && declared.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return declared.GenericTypeArguments[0];
            }

            return declared;
        }

        private static bool IsSuccess(int statusCode) => statusCode >= 200 && statusCode < 300;
    }
}
