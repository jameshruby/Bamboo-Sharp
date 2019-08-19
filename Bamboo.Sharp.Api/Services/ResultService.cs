using Bamboo.Sharp.Api.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.Sharp.Api.Services
{
    public class ResultService : BaseService
    {
        public Results GetResult()
        {
            var request = new RestRequest { Resource = "/result?expand=results.result", Method = Method.GET };
            var resultBase = Client.Execute<ResultsBase>(request);
            return resultBase.Results;
        }

        public Results GetResult(string projectKey)
        {
            var request = new RestRequest { Resource = "/result/{projectKey}?expand=results.result&includeAllStates=true", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);

            var resultBase = Client.Execute<ResultsBase>(request);
            return resultBase.Results;
        }

        public Result GetResultProjBuild(string projectKeyBuildKey)
        {
            var request = new RestRequest { Resource = "/result/{projectKeyBuildKey}-latest?expand=vcsRevisions&includeAllStates=true", Method = Method.GET };
            request.AddParameter("projectKeyBuildKey", projectKeyBuildKey, ParameterType.UrlSegment);
            var resultBase = Client.Execute<Result>(request);
            return resultBase;
        }
        public Result GetResult(string projectKey, string buildKey)
        {
            return GetResultProjBuild(projectKey + "-" + buildKey);
        }

        public Results GetResultNumber(string projectKey, int numberOfResults)
        {
            var request = new RestRequest { Resource = "/result/{projectKey}?expand=results[3:4].result&includeAllStates=true", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            //request.AddParameter("N", numberOfResults, ParameterType.UrlSegment);


            var resultBase = Client.Execute<ResultsBase>(request);
            return resultBase.Results;
        }


        public Results GetResult(string projectKey, int maxResult)
        {
            var request = new RestRequest { Resource = "/result/{projectKey}?expand=results.result&includeAllStates=true&max-result={maxResult}", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("maxResult", maxResult, ParameterType.UrlSegment);

            var resultBase = Client.Execute<ResultsBase>(request);
            return resultBase.Results;

        }
    }
}
