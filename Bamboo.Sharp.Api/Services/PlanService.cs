using Bamboo.Sharp.Api.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.Sharp.Api.Services
{
    public class PlanService : BaseService
    {
        public Plan GetPlans()
        {
            RestRequest request = new RestRequest { Resource = "plan", RootElement = "plan", Method = Method.GET };
            return Client.Execute<Plan>(request);
        }

        public void GetPlansT()
        {
            RestRequest request = new RestRequest { Resource = "plan", RootElement = "plan", Method = Method.GET };
            Client.Execute(request);
        }

        public Plan GetPlan_expanded(string projectKey, string buildKey)
        {
            //RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}?expand=stages.stage.plans.plan.actions", Method = Method.GET };

            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}?expand=plans.pla&expand=variableContext&expand=stages.stage.plans.plan.actions", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            //Client.Execute(request);
            return Client.Execute<Plan>(request);
        }

        public Plan GetPlan(string projectKey, string buildKey)
        {
            //RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}?expand=stages.stage.plans.plan.actions", Method = Method.GET };

            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}?expand=variableContext", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            //Client.Execute(request);
            return Client.Execute<Plan>(request);
        }

        public Plan GetPlanSimple(string projectKey, string buildKey)
        {

            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            //Client.Execute(request);
            return Client.Execute<Plan>(request);
        }

        public bool IsPlanExisting(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            try
            {
                var p = Client.Execute<Plan>(request);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsJobExisting(string projectKey, string buildKey, string jobName)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}?expand=stages.stage.plans.plan.actions", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            var p = Client.Execute<Plan>(request);

            //TODO - use linq var v = p.Stages.All.FirstOrDefault(x => x.Plans.FirstOrDefault(y => y.All.FirstOrDefault(z => z.ShortName == "PACK")));

            bool isFound = false;
            foreach (var stage in p.Stages.All)
            {
                foreach (var plan in stage.Plans)
                {
                    foreach (var job in plan.All)
                    {
                        if (job.ShortKey.Contains(jobName))
                            isFound = true;
                    }
                }
            }

            return isFound;
        }

        public BranchesBase BranchGetAll(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/branch?expand=branches.branch", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
      
            return Client.Execute<BranchesBase>(request);
        }

        public Branch BranchGet(string projectKey, string buildKey, string branchName)
        {
            //retrieving a single branch gives more posibilites as expand = master
            RestRequest request = new RestRequest
            {
                Resource = "plan/{projectKey}-{buildKey}/branch/{branchName}",
                Method = Method.GET
            };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("branchName", branchName, ParameterType.UrlSegment);
            return Client.Execute<Branch>(request);
        }

        public Branch BranchSet(string projectKey, string buildKey, string branchName)
        {

            RestRequest request = new RestRequest
            {
                Resource = "plan/{projectKey}-{buildKey}/branch/{branchName} ",
                Method = Method.PUT
            };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("branchName", branchName, ParameterType.UrlSegment);

            return Client.Execute<Branch>(request);
        }

        public object LabelAdd(string projectKey, string buildKey, string labelValue)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/label", Method = Method.POST };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { name = labelValue });
            return Client.Execute<object>(request);
        }

        public Labells LabelGet(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/label?expand=labels", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            //Client.Execute(request);

            return Client.Execute<Labells>(request);
        }

        public Plan LabelRemove(string projectKey, string buildKey, string labelName)
        {
            RestRequest request = new RestRequest
            {
                Resource = "plan/{projectKey}-{buildKey}/label/{labelName}",
                Method = Method.DELETE
            };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("labelName", labelName, ParameterType.UrlSegment);
            return Client.Execute<Plan>(request);
        }

        public Plan GetPlanVcsBranches(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/vcsBranches ", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<Plan>(request);
        }

        public Plan GetPlanIssue(string projectKey, string buildKey, string issueKey)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/issue/{issueKey} ", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("issueKey", issueKey, ParameterType.UrlSegment);
            Client.Execute(request);
            return Client.Execute<Plan>(request);
        }

        public object FavouritesAdd(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest
            {
                Resource = "plan/{projectKey}-{buildKey}/favourite",
                Method = Method.POST
            };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<object>(request);
        }

        public Plan FavouritesRemove(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest
            {
                Resource = "plan/{projectKey}-{buildKey}/favourite",
                Method = Method.DELETE
            };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<Plan>(request);
        }

        public void PlanEnable(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest
            {
                Resource = "plan/{projectKey}-{buildKey}/enable",
                Method = Method.POST
            };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            Client.Execute(request);
        }

        public void PlanDisable(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest
            {
                Resource = "plan/{projectKey}-{buildKey}/enable",
                Method = Method.DELETE
            };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);

            Client.Execute(request);
        }

        public ArtifactsBase ArtifactsGet(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/artifact?expand=artifacts", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<ArtifactsBase>(request);
        }

        public Plan QuarantineAdd(string projectKey, string buildKey, string testId)
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/test/{testId}/quarantine", Method = Method.POST };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("testId", testId, ParameterType.UrlSegment);
            Client.Execute(request);
            return Client.Execute<Plan>(request);
        }

        public Plan QuarantineUnleash(string projectKey, string buildKey, string testId) 
        {
            RestRequest request = new RestRequest { Resource = "plan/{projectKey}-{buildKey}/test/{testId}/unleash", Method = Method.POST };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            request.AddParameter("testId", testId, ParameterType.UrlSegment);
            return Client.Execute<Plan>(request);
        }

        public FavIcon GetPlanStatusIcon(string projectKey, string buildKey)
        {
            RestRequest request = new RestRequest { Resource = "plan/favicon/{projectKey}-{buildKey}", Method = Method.GET };
            request.AddParameter("projectKey", projectKey, ParameterType.UrlSegment);
            request.AddParameter("buildKey", buildKey, ParameterType.UrlSegment);
            return Client.Execute<FavIcon>(request);
        }

    }
}
