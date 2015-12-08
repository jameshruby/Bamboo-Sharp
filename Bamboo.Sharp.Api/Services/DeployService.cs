﻿using System.Linq;
using System.Threading.Tasks;
using Bamboo.Sharp.Api.Model;
using RestSharp;

namespace Bamboo.Sharp.Api.Services
{
    public class DeployService : BaseService
    {
        #region This methods are not well documented
        //RestRequest request = new RestRequest { Resource = "deploy/result"
        //RestRequest request = new RestRequest { Resource = "deploy/preview
        //RestRequest request = new RestRequest { Resource = "deploy/projectVersioning
        //RestRequest request = new RestRequest { Resource = "deploy/version
        //RestRequest request = new RestRequest { Resource = "deploy/issue-status
        //RestRequest request = new RestRequest { Resource = "deploy/environment 
        #endregion
        public object DeployProject()
        {
            RestRequest request = new RestRequest { Resource = "deploy/project ", Method = Method.PUT };

            return Client.Execute<object>(request);
        }

        public object DeployProject(int id)
        {
            RestRequest request = new RestRequest { Resource = "deploy/project/{id} ", Method = Method.POST };
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetVersionStatus(int id, int state)
        {
            RestRequest request = new RestRequest { Resource = "deploy/version/{id}/status/{state} ", Method = Method.POST };
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddParameter("state", state, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetProject(int id)
        {
            RestRequest request = new RestRequest { Resource = "deploy/project/{id} ", Method = Method.GET };
            return Client.Execute<object>(request);
        }


        public object GetProjectVersioningNextVersion(int id)
        {
            RestRequest request = new RestRequest { Resource = "deploy/projectVersioning/{id}/nextVersion ", Method = Method.GET };
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetProjectVersioningNamingPreview(int id)
        {
            RestRequest request = new RestRequest { Resource = "deploy/projectVersioning/{id}/namingPreview ", Method = Method.GET };
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetProjectVersioningParseVariables(int id)
        {
            RestRequest request = new RestRequest { Resource = "deploy/projectVersioning/{id}/parseVariables ", Method = Method.GET };
            return Client.Execute<object>(request);
        }
        public object GetProjectForPlan(int projectId)
        {
            RestRequest request = new RestRequest { Resource = "deploy/project/{projectId}/versions ", Method = Method.GET };
            request.AddParameter("projectId", projectId, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetProjectForPlan()
        {
            RestRequest request = new RestRequest { Resource = "deploy/project/forPlan ", Method = Method.GET };
            return Client.Execute<object>(request);
        }
        public object GetProjectVersioning(int id)
        {
            RestRequest request = new RestRequest { Resource = "deploy/projectVersioning/{id}/variables ", Method = Method.GET };
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetVersionStatus(int id)
        {
            RestRequest request = new RestRequest { Resource = "deploy/version/{id}/status ", Method = Method.GET };
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetIssueStatus(int key, int deploymentProjectId)
        {
            RestRequest request = new RestRequest { Resource = "deploy/issue-status/{key}/{deploymentProjectId} ", Method = Method.GET };
            request.AddParameter("key", key, ParameterType.UrlSegment);
            request.AddParameter("deploymentProjectId", key, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetIssueStatus(int key)
        {
            RestRequest request = new RestRequest { Resource = "deploy/issue-status/{key} ", Method = Method.GET };
            request.AddParameter("key", key, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public object GetDashboard(int projectId)
        {
            RestRequest request = new RestRequest { Resource = "deploy/dashboard/{projectId} ", Method = Method.GET };
            request.AddParameter("projectId", projectId, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public void GetDashboard()
        {
            RestRequest request = new RestRequest { Resource = "deploy/dashboard ", Method = Method.GET };
            Client.Execute(request);
        }
        public object GetEnvironment(int environmentId)
        {
            RestRequest request = new RestRequest { Resource = "deploy/environment/{environmentId}/results ", Method = Method.GET };
            request.AddParameter("environmentId", environmentId, ParameterType.UrlSegment);

            return Client.Execute<object>(request);
        }

        public object GetPreviewVersion()
        {
            RestRequest request = new RestRequest { Resource = "deploy/preview/versionName ", Method = Method.GET };
            return Client.Execute<object>(request);
        }

        public object GetPreviewPossibleResult()
        {
            RestRequest request = new RestRequest { Resource = "deploy/preview/possibleResults ", Method = Method.GET };
            return Client.Execute<object>(request);
        }

        public object GetPreviewResult()
        {
            RestRequest request = new RestRequest { Resource = "deploy/preview/result ", Method = Method.GET };
            return Client.Execute<object>(request);
        }

        public object GetVersion()
        {
            RestRequest request = new RestRequest { Resource = "deploy/preview/version ", Method = Method.GET };
            return Client.Execute<object>(request);
        }

        public object GetResult(int deploymentResultId)
        {
            RestRequest request = new RestRequest { Resource = "deploy/result/{deploymentResultId} ", Method = Method.GET };
            request.AddParameter("deploymentResultId", deploymentResultId, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }
    }
}
