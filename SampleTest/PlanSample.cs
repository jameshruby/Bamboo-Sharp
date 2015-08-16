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
        }
    }
}
