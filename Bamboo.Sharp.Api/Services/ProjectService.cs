using System.Linq;
using System.Threading.Tasks;
using Bamboo.Sharp.Api.Model;
using RestSharp;
using System;

namespace Bamboo.Sharp.Api.Services
{
    public class ProjectService : BaseService
    {
        //Supported resources
        private const string GetProjectsResource = "project?expand=projects.project.plans.plan.actions";
        private const string CloneResource = "clone/{projectKey}-{buildKey}:{toProjectKey}-{toBuildKey}";

        //Already implemented via linq
        //private const string GetProjectsResourceWithKey = "project/{projectKey}";

        //Base requests
        private readonly IRestRequest _baseGetProjectsRequest = new RestRequest
        {
            Resource = GetProjectsResource,
            RootElement = "projects",
            Method = Method.GET
        };

        private readonly IRestRequest _baseCloneRequest = new RestRequest
        {
            Resource = CloneResource,
            Method = Method.PUT
        };

        //Implemenations

        public void GetProjectsDebug()
        {
            RestRequest request = new RestRequest { Resource = "project/{projectKey}?expand", Method = Method.GET };
            request.AddParameter("projectKey", "AIIIDATA", ParameterType.UrlSegment);
            Client.Execute(_baseGetProjectsRequest);
        }

        public Projects GetProjects()
        {
            return Client.Execute<Projects>(_baseGetProjectsRequest);
        }

        // with bigger size of project will increase a time
        public Project GetProjectWithAllPlans(string projectKey)
        {
            RestRequest request = new RestRequest { Resource = "project/{projectKey}", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);

            int plansCount = Client.Execute<Project>(request).Plans.Size;

            request = new RestRequest { Resource = "project/{projectKey}?expand=plans.plan.actions&max-result=" + plansCount, Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);

            return Client.Execute<Project>(request);
        }
        
        //duplicated
        public Project GetProjectWithAllPlansAndVariables(string projectKey)
        {
            RestRequest request = new RestRequest { Resource = "project/{projectKey}", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);

            int plansCount = Client.Execute<Project>(request).Plans.Size;
            //.actions,plans.plan.variableContext
            request = new RestRequest { Resource = "project/{projectKey}?expand=plans&max-result=" + plansCount, Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);

            //Client.Execute(request);    
            return Client.Execute<Project>(request);
        }



        public Project GetProjectWithPlanNames(string projectKey)
        {
            RestRequest request = new RestRequest { Resource = "project/{projectKey}?expand=plans.plan.actions", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);

            return Client.Execute<Project>(request);
        }

        public async Task<Projects> GetProjectsAsync()
        {
            return await Client.ExecuteAsync<Projects>(_baseGetProjectsRequest);
        }

        public Project GetProject(string key)
        {
            return GetProjects().All.FirstOrDefault(p => p.Key.Equals(key));
        }

        public async Task<Project> GetProjectAsync(string key)
        {
            var projects = await GetProjectsAsync();
            return projects.All.FirstOrDefault(p => p.Key.Equals(key));
        }

        public Plan Clone(string fromProjKey, string fromBuildKey, string newProjectKey, string newBuildKey)
        {
            _baseCloneRequest.AddParameter("projectKey", fromProjKey, ParameterType.UrlSegment);
            _baseCloneRequest.AddParameter("buildKey", fromBuildKey, ParameterType.UrlSegment);
            _baseCloneRequest.AddParameter("toProjectKey", newProjectKey, ParameterType.UrlSegment);
            _baseCloneRequest.AddParameter("toBuildKey", newBuildKey, ParameterType.UrlSegment);
            _baseCloneRequest.AddParameter("name", "enabled", ParameterType.QueryString);
            _baseCloneRequest.AddParameter("shortName", "enabled", ParameterType.QueryString);

            Client.Execute(_baseCloneRequest);
            return new Plan();

            //return Client.Execute<Plan>(_baseCloneRequest);
        }

        public void TestChangeConfig()
        {
            var testRequest = new RestRequest
            {
                Resource = "buildKey=JHT-AN&projectName=Jakub+Hruby+Tests&chainName=Anims_f_Exp3&chainDescription=&enabled=true&checkBoxFields=enabled&returnUrl=&planKey=JHT-AN&save=Save", //?buildKey={buildKey}&projectName={projectName}&chainName={chainName}
                Method = Method.POST
            };

            //testRequest.AddObject(new
            //{
            //    buildKey = "JHT-AN",
            //    projectName = "Jakub+Hruby+Tests",
            //    chainName = "Anims_f_Exp3",
            //    chainDescription = "",
            //    enabled = "true",
            //    checkBoxFields = "enabled",
            //    returnUrl = "",
            //    planKey = "JHT-AN",
            //    save = "Save"
            //});

            //testRequest.AddParameter("buildKey", "JHT-AN");
            //testRequest.AddParameter("projectName", "JakubHrubyTests");
            //testRequest.AddParameter("chainName", "Anims_f_Exp3");

            ////testRequest.AddParameter("chainDescription", "");
            ////testRequest.AddParameter("enabled", "true");
            ////testRequest.AddParameter("checkBoxFields", "enabled");
            ////testRequest.AddParameter("returnUrl", "");
            //testRequest.AddParameter("planKey", "JHT-AN");
            //testRequest.AddParameter("save", "Save");
     
         
            Client.Execute(testRequest);
          

            //return Client.Execute<Plan>(_baseCloneRequest);
        }

        public async Task<Plan> CloneAsync(string fromProjKey, string fromBuildKey, string newProjectKey, string newBuildKey)
        {
            _baseCloneRequest.AddParameter("projectKey", fromProjKey, ParameterType.UrlSegment);
            _baseCloneRequest.AddParameter("buildKey", fromBuildKey, ParameterType.UrlSegment);
            _baseCloneRequest.AddParameter("toProjectKey", newProjectKey, ParameterType.UrlSegment);
            _baseCloneRequest.AddParameter("toBuildKey", newBuildKey, ParameterType.UrlSegment);

            return await Client.ExecuteAsync<Plan>(_baseCloneRequest);
        }
    }
}
