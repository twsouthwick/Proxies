using Proxies.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxySample.Models
{
    public class ValueModel
    {
        [Translate]
        public string Item1 { get; set; }

        public string Item2 { get; set; }
    }
}
