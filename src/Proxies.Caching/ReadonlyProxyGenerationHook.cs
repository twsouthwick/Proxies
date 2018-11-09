﻿using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Proxies.Caching
{
    internal class ReadonlyProxyGenerationHook : IProxyGenerationHook
    {
        private readonly ILogger<ReadonlyProxyGenerationHook> _logger;

        public ReadonlyProxyGenerationHook(ILogger<ReadonlyProxyGenerationHook> logger)
        {
            _logger = logger;
        }

        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttribute<CachedAttribute>() != null && IsTask(methodInfo);
        }

        private bool IsTask(MethodInfo methodInfo)
        {
            var returnType = methodInfo.ReturnType;

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return true;
            }

            _logger.LogWarning("Invalid return type '{Type}' on '{MemberInfo}' for caching", returnType, methodInfo);

            return false;
        }
    }
}
