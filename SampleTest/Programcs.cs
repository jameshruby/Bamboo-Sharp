using Bamboo.Sharp.Api;
using Bamboo.Sharp.Api.Services;
using Bamboo.WebRequests.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        private static PlanService planService;
        private static QueueService queueService;
        private static WebApi wa;

        static void Main(string[] args)
        {
            //Console.WriteLine("Processing...");

            const string apiBaseUrl = @"https://bamboo.bistudio.com/rest/api/latest/";
            #region MyRegion
            const string userName = "";
            const string password = "";
            #endregion
            BambooApi api = new BambooApi(apiBaseUrl, userName, password);
          
            wa = new WebApi(userName, password);
            wa.SimpleHttp.Verbose = true;
        
            var projectsService = api.GetService<ProjectService>();

            planService = api.GetService<PlanService>();
            queueService = api.GetService<QueueService>();
        

            //var plans = planService.GetPlan("AIIIDATA", "STRUCDEX");

            //projectsService.Clone("JHT", "AH", "JHT", "AIR11");
            ////projectsService.TestChangeConfig();

            var templateProjKey = "JHT";
            var projKey = "AIIIDATA";
            var templatebuildKey = "TEM";


            var tetest = projectsService.GetProjectWithAllPlans(templateProjKey);
            foreach (var plan in tetest.Plans.All)
            {
                if (plan.ShortKey == "RRRDEN")
                {
                    //wa.ShareAllArtifactsToAnotherJob(plan.ProjectKey, plan.ShortKey, "JOB1", "RRRR");
                    //wa.DeleteStage(plan.ProjectKey, plan.ShortKey, "GetSvnConfiguration");
                    wa.AddPlanVariable(plan.ProjectKey, plan.ShortKey, "aaaa", "cc");
                }
            }

            var arma3DataPlansWitVars = projectsService.GetProjectWithAllPlansAndVariables(templateProjKey);
            foreach (var plan in arma3DataPlansWitVars.Plans.All)
            {
                if (plan.ShortName.ToLower().Contains("exp"))
                {

                    var currentPlan = planService.GetPlan(projKey, plan.ShortKey);
                    foreach (var variable in currentPlan.VariableContext.All)
                    {
                        if (variable.Key == "a3_useOldBuildingPipeline" && variable.Value == "true")
                        {
                            Console.WriteLine(plan.ShortName + Environment.NewLine);
                        }
                    }
                }
            }

            var arma3DataPlans = projectsService.GetProjectWithAllPlans(projKey);
            var buildingPlans = new List<Bamboo.Sharp.Api.Model.Plan>();
            var processed = true;

            foreach (var plan in arma3DataPlans.Plans.All)
            {
                var buildKey = plan.ShortKey;
                Console.WriteLine(plan.ProjectKey + " " + plan.ShortKey + " " + plan.ShortName);

                foreach (var item in plan.VariableContext.All)
                {
                    Console.WriteLine("{0} {1}", item.Key, item.Value);
                }


                if (plan.ShortKey == "RNDROOT")
                {

                }
                //else if (plan.ShortKey == "MAPSDT")
                //{
                //    processed = false;
                //}
                else if (plan.IsBuilding)
                {
                    //buildingPlans.Add(plan);
                }

                    //STRUCTURES_F_EXP || MAP_TANOABUKA

                else if (plan.ShortKey == "STRUCEX")// if (plan.BuildName.ToLower().Contains("exp"))
                {

                    //DeletePlan(plan.ProjectKey, plan.ShortKey);
                    //ClonePlan("AIIIDATA", plan.ShortKey, plan.ProjectKey, plan.ShortKey);


                    //DeleteStage(plan.ProjectKey, buildKey, "Build package");
                    //CreateStage(plan.ProjectKey, plan.ShortKey, "Build+package");
                    //CreateStage(plan.ProjectKey, plan.ShortKey, "Synchronize+data");


                    //wa.JobCleanWorkingDirectory(plan.ProjectKey, buildKey, "JOB1");
                    //wa.DeleteJob(plan.ProjectKey, buildKey, "DTF");

                    //CloneJob(templateProjKey, templatebuildKey, "JOB1", "Build+Package", plan.ProjectKey, buildKey, "Build+package");

                    //CloneJob(templateProjKey, templatebuildKey, "RRRR", "Copy+data+and+packlogs", plan.ProjectKey, buildKey, "Synchronize+data");
                    //CloneJob(templateProjKey, templatebuildKey, "DTF", "Delete+temp+files", plan.ProjectKey, buildKey, "Synchronize+data");

                    //ShareAllArtifactsToAnotherJob(plan.ProjectKey, buildKey, "JOB1", "RRRR");
                    //AddPlanVariable(plan.ProjectKey, buildKey, "a3_useOldBuildingPipeline", "false");
                    //AddPlanVariable(plan.ProjectKey, buildKey, "svnSubdirectory");

                }
            }


            /*
            for plan cloning, load all plans for arma3data project
            to array and than foreach it,
            
            */

            //api.GetService<QueueService>().Show();

            //planService.RemovePlanFromFavourites("JHT", "AIR");




            //planService.LabelAdd("JHT", "AIR");
            //var label = planService.LabelGet("JHT", "AIR");


            //var projectsService = api.GetService<ProjectService>();
            //var projects = projectsService.GetProjects();
            //Console.WriteLine("Alive!");

            //var project = projectsService.GetProject("JHT");

            //var queue = api.GetService<QueueService>();
            // var user = api.GetService<CurrentUserService>();
        }
    }
}
