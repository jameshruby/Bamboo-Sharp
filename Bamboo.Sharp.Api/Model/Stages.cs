using RestSharp.Deserializers;
using System.Collections.Generic;
namespace Bamboo.Sharp.Api.Model
{
    public class Stages : BaseNode
    {
        [DeserializeAs(Name = "stage")]
        public List<Stage> All { get; set; }
    }
}
