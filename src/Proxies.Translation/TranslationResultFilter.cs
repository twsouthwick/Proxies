using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Proxies.Translation
{
    internal class TranslationResultFilter : IAsyncResultFilter
    {
        private readonly ConcurrentDictionary<Type, IObjectTranslator> _translators;
        private readonly ILogger<TranslationResultFilter> _logger;

        public TranslationResultFilter(ILogger<TranslationResultFilter> logger)
        {
            _translators = new ConcurrentDictionary<Type, IObjectTranslator>();
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (IsSuccess(context.HttpContext.Response.StatusCode))
            {
                await UpdateValue(context.Result, context.HttpContext);
            }

            await next();
        }

        private async Task UpdateValue(IActionResult result, HttpContext context)
        {
            if (result is ObjectResult objResult && _translators.GetOrAdd(objResult.DeclaredType, CreateTranslator, context.RequestServices) is IObjectTranslator translator)
            {
                objResult.Value = await translator.TranslateAsync(objResult.Value, context.RequestAborted).ConfigureAwait(false);
            }
        }

        private IObjectTranslator CreateTranslator(Type type, IServiceProvider services)
        {
            var expectedType = typeof(ObjectTranslator<>).MakeGenericType(UnwrapType(type));
            var translator = (IObjectTranslator)services.GetService(expectedType);

            if (translator.Count > 0)
            {
                _logger.LogInformation("Create object translator for '{Type}' with {PropertyCount} translatable properties", type, translator.Count);

                return translator;
            }
            else
            {
                _logger.LogDebug("No translatable properties on '{Type}'", type);

                return null;
            }
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
