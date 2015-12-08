using RestSharp.Deserializers;
using System.Collections.Generic;
namespace Bamboo.Sharp.Api.Model
{
    public class ResultsBase :BaseNode
    {
        public Results Results { get; set; }
    }

    public class Results : BaseNode
    {
        [DeserializeAs(Name = "result")]
        public List<Result> All { get; set; }
    }
}
