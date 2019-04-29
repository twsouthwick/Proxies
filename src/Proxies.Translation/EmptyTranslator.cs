﻿using System.Threading.Tasks;

namespace Proxies.Translation
{
    internal class EmptyTranslator : ITranslator
    {
        public Task<string> TranslateAsync(string input) => Task.FromResult(input);
    }
}
