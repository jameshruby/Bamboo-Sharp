using System.Linq;
using System.Threading.Tasks;
using Bamboo.Sharp.Api.Model;
using RestSharp;

namespace Bamboo.Sharp.Api.Services
{
    public class DependencyService : BaseService
    {
        public object DependencyChild(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "dependency/{projectKey}-{buildKey}/child ", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }


        public object DependencySearchChild(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "dependency/search/{projectKey}-{buildKey}/child ", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object DependencyParent(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "dependency/{projectKey}-{buildKey}/parent ", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }


        public object DependencySearchParent(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "dependency/search/{projectKey}-{buildKey}/parent ", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }
    }
}
