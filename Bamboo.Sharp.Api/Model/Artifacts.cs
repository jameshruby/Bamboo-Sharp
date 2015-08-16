using Bamboo.Sharp.Api.Model;
using RestSharp.Deserializers;
using System.Collections.Generic;

namespace Bamboo.Sharp.Api.Model
{
    public class Artifact
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public bool shared { get; set; }
        public string copyPattern { get; set; }
    }
    
    public class Artifacts : BaseNode
    {
        public int size { get; set; }

        [DeserializeAs(Name = "artifact")]
        public List<Artifact> All { get; set; }
    }

    public class ArtifactsBase
    {
        //public string expand { get; set; }
        //public Link link { get; set; }
        public Artifacts artifacts { get; set; }
    }
}
