using Bamboo.Sharp.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest
{
    public class PlanSample
    {
        public PlanSample(Bamboo.Sharp.Api.BambooApi api, string projectKey, string buildKey)
        {
            var planService = api.GetService<PlanService>();

            var pland = planService.GetPlan_expanded(projectKey, buildKey);
            //foreach (var planVar in pland.VariableContext.All)
            //{
            //     Console.WriteLine("{0} = {1}",planVar.Key, planVar.Value);  
            //}

            //var r = planService.GetPlanStatusIcon(projectKey, buildKey);

            //planService.QuarantineAdd("AIIIDATA", "A3BI", "647");
            //planService.QuarantineUnleash(projectKey, "A3BI", "647");

            #region Artifacts
            var artifacts = planService.ArtifactsGet(projectKey, buildKey);
            foreach (var artifact in artifacts.artifacts.All)
            {
                Console.WriteLine();
                Console.WriteLine("Artifact Name: {0}", artifact.name);
                Console.WriteLine("Artifact Id: {0}", artifact.id);
                Console.WriteLine("Artifact Location: {0}", artifact.location);
                Console.WriteLine("Artifact Shared: {0}", artifact.shared);
                Console.WriteLine("Artifact CopyPattern: {0}", artifact.copyPattern);
            }

            #endregion

            #region Favourites
            var planExpanded = planService.GetPlanSimple(projectKey, buildKey);
            if (planExpanded.IsFavourite)
                planService.FavouritesRemove(projectKey, buildKey);
            else
                planService.FavouritesAdd(projectKey, buildKey);
            #endregion

            #region Branches
            //var branchName = "ILoveTheMonkeyHead";
            //var newBranch = planService.BranchSet(projectKey, buildKey, branchName);
            //var selectedBranches = planService.BranchGet(projectKey, buildKey, branchName);

            //Console.WriteLine("Branch Name: {0}", selectedBranches.Name);
            //Console.WriteLine("Branch ShortKey: {0}", selectedBranches.ShortKey);
            //Console.WriteLine("Branch ShortName: {0}", selectedBranches.ShortName);
            //Console.WriteLine("Branch Key: {0}", selectedBranches.Key);
            //Console.WriteLine("Branch Enabled: {0}", selectedBranches.Enabled);
            #endregion
        }
    }
}
