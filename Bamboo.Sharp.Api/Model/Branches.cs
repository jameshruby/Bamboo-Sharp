using System.Collections.Generic;

namespace Bamboo.Sharp.Api.Model
{
    public class Branch
    {
        public string Description { get; set; }
        public string ShortName { get; set; }
        public string ShortKey { get; set; }
        public bool Enabled { get; set; }
        public Link Link { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }

    public class Branches : BaseNode
    {
        public int size { get; set; }
        [RestSharp.Deserializers.DeserializeAs(Name = "branch")]
        public List<Branch> All { get; set; }
    }

    public class BranchesBase
    {
        public string expand { get; set; }
        public Link link { get; set; }
        public Branches branches { get; set; }
    }
    
}
