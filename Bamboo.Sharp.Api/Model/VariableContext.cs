using RestSharp.Deserializers;
using System.Collections.Generic;
namespace Bamboo.Sharp.Api.Model
{
    public class VariableContext : BaseNode
    {
        [DeserializeAs(Name = "variable")]
        public List<Variable> All { get; set; }
    }
}
