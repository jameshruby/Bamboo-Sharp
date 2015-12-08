using Bamboo.Sharp.Api;
using Bamboo.Sharp.Api.Services;
using Bamboo.WebRequests.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest
{
    class Program
    {
        private static PlanService planService;
        private static QueueService queueService;
        private static WebApi wa;
        
        private static void PlanUpdate(Bamboo.Sharp.Api.Model.Plan plan)
        {
            Console.WriteLine(plan.Name);
            //var templateProjKey = "JHT";
            //var projKey = "AIIIDATA";
            //var templatebuildKey = "EDEN"; //TEM

            //var r = planService.GetPlan_expanded(plan.ProjectKey, plan.ShortKey);
            //if (plan.ShortKey != "MODEP" && plan.ShortKey != "LANB" && plan.ShortKey != "MISFDDD" && plan.ShortKey != "DDENDT")
            //{
            //wa.DeleteJob(plan.ProjectKey, plan.ShortKey, "JOB1");
            //planService.BranchSet(plan.ProjectKey, plan.ShortKey, "A3-VisualUpdate");

            //wa.OverrideNthBranchRepository(plan.ProjectKey, plan.ShortKey, 0);
            //wa.AddPlanVariable(plan.ProjectKey, plan.ShortKey, "useBranch");

            //wa.AddPlanVariable(plan.ProjectKey, plan.ShortKey, "useBranch", 0);

            //wa.CloneJob("JHT", "ARMORF", "JOB1", "Build+package", plan.ProjectKey, plan.ShortKey, "Build package");
            //}
        }
            static void Main(string[] args)
        {
            //Console.WriteLine("Processing...");

            const string apiBaseUrl = @"https://bamboo.bistudio.com/rest/api/latest/";
            #region MyRegion
            const string userName = "hrubyjak";
            const string password = "bi4T$gr";
            #endregion
            BambooApi api = new BambooApi(apiBaseUrl, userName, password);

            //var resultService = new ResultService();

            //var r = resultService.GetResultNumber("JHT", 10);
            //r.All.OrderByDescending(c => c.BuildCompletedDate.Date)
            // .ThenBy(c => c.BuildCompletedDate.TimeOfDay);

            //wa = new WebApi(userName, password);
            //wa.SimpleHttp.Verbose = true;

            var projectsService = api.GetService<ProjectService>();
            
            //planService = api.GetService<PlanService>();

            var arma3DataPlans = projectsService.GetProjectWithAllPlans("JHT");

            //planService.GetPlan_expanded("AIIIDATA", "ARMORF");
            //queueService = api.GetService<QueueService>();
            //projectsService.Clone("JHT", "AH", "JHT", "AIR11");
            ////projectsService.TestChangeConfig();

            var templateProjKey = "JHT";
            var projKey = "AIIIDATA";
            var templatebuildKey = "EDEN"; //TEM


            //var tetest = projectsService.GetProjectWithAllPlans(templateProjKey);
            //foreach (var plan in tetest.Plans.All)
            //{
            //    if (plan.ShortKey == "RRRDEN")
            //    {
            //        //wa.ShareAllArtifactsToAnotherJob(plan.ProjectKey, plan.ShortKey, "JOB1", "RRRR");
            //        //wa.DeleteStage(plan.ProjectKey, plan.ShortKey, "GetSvnConfiguration");
            //        wa.AddPlanVariable(plan.ProjectKey, plan.ShortKey, "aaaa", "cc");
            //    }
            //}

            //var arma3DataPlansWitVars = projectsService.GetProjectWithAllPlansAndVariables(templateProjKey);
            //foreach (var plan in arma3DataPlansWitVars.Plans.All)
            //{
            //    if (plan.ShortName.ToLower().Contains("exp"))
            //    {

            //        var currentPlan = planService.GetPlan(projKey, plan.ShortKey);
            //        foreach (var planVar in currentPlan.VariableContext.All)
            //        {
            //            if (planVar.Key == "a3_useOldBuildingPipeline" && planVar.Value == "true")
            //            {
            //                Console.WriteLine(plan.ShortName + Environment.NewLine);
            //            }
            //        }
            //    }
            //}


            //var arma3DataPlans = projectsService.GetProjectWithAllPlans(projKey);
            //var buildingPlans = new List<Bamboo.Sharp.Api.Model.Plan>();
            //var processed = true;

            //arma3DataPlans.Plans.All.ForEach(p => PlanUpdate(p));

            //foreach (var plan in arma3DataPlans.Plans.All)
            //{
            //    var buildKey = plan.ShortKey;
            //    if (plan.ShortKey == "MISDF")
            //    {
            //        processed = false;
            //    }
            //    //else if (plan.ShortKey == "MAPSDT")
            //    //{
            //    //    processed = false;
            //    //}
            //    else if (plan.IsBuilding)
            //    {
            //        buildingPlans.Add(plan);
            //    }

            //    //STRUCTURES_F_EXP || MAP_TANOABUKA

            //    else if (!processed)
            //    {
            //        //Console.WriteLine(plan.ProjectKey + " " + plan.ShortKey + " " + plan.ShortName);

            //        //wa.DeleteTask

            //        //wa.DeleteJob(plan.ProjectKey, buildKey, "JOB1");
            //        //wa.DeleteStage(plan.ProjectKey, buildKey, "Copy data and packlogs");

            //        //wa.CloneJob(templateProjKey, templatebuildKey, "JOB1", "Build+package", plan.ProjectKey, buildKey, "Build package");

            //        //wa.ShareAllArtifactsToAnotherJob(plan.ProjectKey, buildKey, "JOB1", "RRRR");

            //        //wa.DeletePlan(plan.ProjectKey, plan.ShortKey);
            //        //wa.ClonePlan("JHT", templatebuildKey, plan.ProjectKey, plan.ShortKey);


            //        //DeleteStage(plan.ProjectKey, buildKey, "Build package");
            //        //CreateStage(plan.ProjectKey, plan.ShortKey, "Build+package");
            //        //CreateStage(plan.ProjectKey, plan.ShortKey, "Synchronize+data");


            //        //wa.JobCleanWorkingDirectory(plan.ProjectKey, buildKey, "JOB1");


            //        //CloneJob(templateProjKey, templatebuildKey, "RRRR", "Copy+data+and+packlogs", plan.ProjectKey, buildKey, "Synchronize+data");
            //        //CloneJob(templateProjKey, templatebuildKey, "DTF", "Delete+temp+files", plan.ProjectKey, buildKey, "Synchronize+data");

            //        //AddPlanVariable(plan.ProjectKey, buildKey, "svnSubdirectory");
            //    }
            //}


            /*
            for plan cloning, load all plans for arma3data project
            to array and than foreach it,
            
            */

            //api.GetService<QueueService>().Show();

            //var projectsService = api.GetService<ProjectService>();
            //var projects = projectsService.GetProjects();
            //Console.WriteLine("Alive!");

            //var project = projectsService.GetProject("JHT");

            //var queue = api.GetService<QueueService>();
            // var user = api.GetService<CurrentUserService>();
        }
    }
}
