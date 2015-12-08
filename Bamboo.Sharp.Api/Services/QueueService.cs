using Bamboo.Sharp.Api.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.Sharp.Api.Services
{
   public class QueueService : BaseService
    {
        public void Show()
        {
            RestRequest request = new RestRequest { Resource = "queue?expand=queuedBuilds.queuedBuild", Method = Method.GET };

            //Client.Execute(request);
            var r = Client.Execute<RootObject>(request);
        }
        
        public void Add(string projKey, string buildKey)
        {
            RestRequest request = new RestRequest
            {
                Resource = "queue/{projectKey}-{buildKey}",
                Method = Method.POST
            };
            request.AddParameter("projectKey", projKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            Client.Execute<object>(request);
        }

        public void Add(string projKey, string buildKey, int buildNumber)
        {
            RestRequest request = new RestRequest { Resource = "queue/{projectKey}-{buildKey}-{buildNumber : ([0-9]+)} ", Method = Method.PUT };

            request.AddParameter("projectKey", projKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("buildNumber", buildNumber, ParameterType.UrlSegment);

            var r = Client.Execute<object>(request);
        }
       
        public void Add(string projKey, string buildKey, string stageName)
        {
            RestRequest request = new RestRequest { Resource = "queue/{projectKey}-{buildKey}?stage={stageName}?executeAllStages=false", Method = Method.POST };

            request.AddParameter("projectKey", projKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("stageName", stageName, ParameterType.UrlSegment);

            var r = Client.Execute<object>(request);
        }

        public void Remove(string projKey, string buildKey, int buildNumber)
        {
            RestRequest request = new RestRequest { Resource = "queue/{projectKey}-{buildKey}-{buildNumber}", Method = Method.DELETE };

            request.AddParameter("projectKey", projKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("buildNumber", buildNumber, ParameterType.UrlSegment);

            var r = Client.Execute<object>(request);
        }
    }
}
