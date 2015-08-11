using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bamboo.Sharp.Api.Model
{
    public class Variable
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string VariableType { get; set; }
        public bool IsPassword { get; set; }
    }
}
