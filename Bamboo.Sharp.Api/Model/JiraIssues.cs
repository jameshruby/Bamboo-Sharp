using RestSharp.Deserializers;
using System.Collections.Generic;
using System.Security.Policy;
namespace Bamboo.Sharp.Api.Model
{
    public class JiraIssues : BaseNode
    {
        [DeserializeAs(Name = "issue")]
        public List<Issue> all { get; set; }
    }

    public class Issue
    {
        [DeserializeAs(Name = "-key")]
        public string Key { get; set; }
     
        [DeserializeAs(Name = "-summary")]
        public string Summary { get; set; }
        
        [DeserializeAs(Name = "-iconUrl")]
        public string IconUrl { get; set; }
        
        [DeserializeAs(Name = "-issueType")]
        public string IssueType { get; set; }
        
        [DeserializeAs(Name = "-status")]
        public string Status { get; set; }
        
        [DeserializeAs(Name = "-statusIconUrl")]
        public string StatusIconUrl { get; set; }
        
        public Url url { get; set; }
    }
}
