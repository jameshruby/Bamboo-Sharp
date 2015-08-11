using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.Sharp.Api.Model
{
    //class QueuedBuild : BaseNode
    //{
    //    public string PlanKey { get; set; }
    //    public int BuildNumber { get; set; }
    //    public string BuildResultKey { get; set; }
    //    public string TriggerReason { get; set; } //enum with "Manual build" and probably Automatic Build?
    //    public Link Link { get; set; }
    //}
    //class QueuedBuilds : BaseNode
    //{
    //    [DeserializeAs(Name = "queuedBuild")]
    //    public List<QueuedBuild> All { get; set; }
    //}



    public class Link2
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

    public class QueueDBuild
    {
        public string planKey { get; set; }
        public int buildNumber { get; set; }
        public string buildResultKey { get; set; }
        public string triggerReason { get; set; }
        public Link2 link { get; set; }
    }
    public class QueuedBuilds : BaseNode
    {
        public string expand { get; set; }
        //[DeserializeAs(Name = "start-index")]
        public List<QueueDBuild> queuedBuild { get; set; }
    }
    public class RootObject
    {
        public string expand { get; set; }
        public Link link { get; set; }
        public QueuedBuilds queuedBuilds { get; set; }
    }
}
