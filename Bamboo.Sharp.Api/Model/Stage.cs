using RestSharp.Deserializers;
using System.Collections.Generic;
namespace Bamboo.Sharp.Api.Model
{
    public class Stage : BaseNode
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //[DeserializeAs(Name = "plans")]
        public List<Plans> Plans { get; set; }
        public string Manual { get; set; }
    }
}
