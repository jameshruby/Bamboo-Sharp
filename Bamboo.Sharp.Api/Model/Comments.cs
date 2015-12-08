using RestSharp.Deserializers;
using System.Collections.Generic;
namespace Bamboo.Sharp.Api.Model
{
    public class Comments : BaseNode
    {

        public List<Comment> comment { get; set; }
    }

    public class Comment
    {
        [DeserializeAs(Name = "-author")]
        public string Author { get; set; }
        public string Content { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
    }
}
