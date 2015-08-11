using Bamboo.Sharp.Api.Model;
using RestSharp.Deserializers;
using System.Collections.Generic;

namespace Bamboo.Sharp.Api.Model
{
    public class Label
    {
    public string name { get; set; }
    }

    public class Labels : BaseNode
    {
    public int size { get; set; }
    public List<Label> label { get; set; }
    }

    public class Labells
    {
    public string expand { get; set; }
    public Link link { get; set; }
    public Labels labels { get; set; }
    }

    //public class Labels
    //{
    //    public string expand { get; set; }

    //    public Link Link { get; set; }

    //    [DeserializeAs(Name = "labels")]
    //    public List<Label> All { get; set; }
    //}
    //`
    //{
    //    public string Name { get; set; }
    //}
}
