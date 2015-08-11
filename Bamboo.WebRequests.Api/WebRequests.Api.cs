using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bamboo.WebRequests.Api
{
    public class WebApi
    {
        //atl token matches cookies
        private string atl_token = "0e5679088f2b8975a5d619f4dba4d2b532168eb1";
        private string password;
        private string login;
        private SimpleHttpHelper simpleHttp;

        public SimpleHttpHelper SimpleHttp { get { return simpleHttp; } set { simpleHttp = value; } }
        public string Atl_token { get { return atl_token; } set { atl_token = value; } }

        public WebApi(string login, string password)
        {
            this.simpleHttp = new SimpleHttpHelper(login, password, atl_token);
        }

        public void ClonePlan(string projKey, string buildKey, string clonedPlanKey, string clonedPlanName)
        {
            string request = @"https://bamboo.bistudio.com/build/admin/create/performClonePlan.action";
            string postParams = string.Format("planKeyToClone={0}-{1}&selectFields=planKeyToClone&existingProjectKey={0}&selectFields=existingProjectKey&projectName=&projectKey=&chainName={2}&chainKey={3}&chainDescription=&clonePlan=true&tmp.createAsEnabled=true&checkBoxFields=tmp.createAsEnabled&save=Create&atl_token=" + atl_token, projKey, buildKey, clonedPlanName, clonedPlanKey);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void DeletePlan(string projKey, string buildKey)
        {
            string request = @"https://bamboo.bistudio.com/chain/admin/deleteChain!doDelete.action";
            string postParams = string.Format("buildKey={0}-{1}&returnUrl=&save=Confirm&atl_token=" + atl_token, projKey, buildKey);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void CreateStage(string projectKey, string planKey, string stageName)
        {
            string request = @"https://bamboo.bistudio.com/chain/admin/ajax/createStage.action";
            string postParams = string.Format("returnUrl=&stageName={0}&stageDescription=&checkBoxFields=stageManual&buildKey={1}-{2}&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true", stageName, projectKey, planKey);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void CloneJob(string projKey, string buildKey, string jobToCloneKey, string jobToCloneName, string targetProjKey, string targetBuildKey, string existingStage)
        {
            string request = @"https://bamboo.bistudio.com/chain/admin/createClonedJob.action";
            string postParams = string.Format("chainKeyToClone={0}-{1}&selectFields=chainKeyToClone&jobKeyToClone={0}-{1}-{2}&selectFields=jobKeyToClone&existingStage={6}&buildName={3}&subBuildKey={2}&buildDescription=&tmp.createAsEnabled=true&checkBoxFields=tmp.createAsEnabled&buildKey={4}-{5}&cloneJob=true&ignoreUnclonableSubscriptions=true&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true"
                                    , projKey, buildKey, jobToCloneKey, jobToCloneName, targetProjKey, targetBuildKey, existingStage);

            while (!simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult())
            {
                System.Threading.Thread.Sleep(500);
            }
        }

        public void DeleteJob(string projKey, string buildKey, string jobKey)
        {
            string request = @"https://bamboo.bistudio.com/chain/admin/deleteChain!doDelete.action";
            string postParams = string.Format("buildKey={0}-{1}-{2}&returnUrl=&save=Confirm&atl_token=" + atl_token, projKey, buildKey, jobKey);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void DeleteStage(string projKey, string buildKey, string stageName)
        {
            string requestR = String.Format("https://bamboo.bistudio.com/chain/admin/config/defaultStages.action?buildKey={0}-{1}", projKey, buildKey);
            var stagesWebPage = simpleHttp.ExecuteGetRequest(requestR).GetAwaiter().GetResult();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(stagesWebPage);

            var elementName = "stage-";
            var selectedStage = htmlDoc.DocumentNode.Descendants()
                                            .SingleOrDefault(element => element.Id.Contains(elementName) &&
                                                             element.Descendants().Any(childElement => childElement.InnerText == stageName)
                                                            ).Id;

            int stageId = Int32.Parse(selectedStage.Trim(elementName.ToCharArray()));

            string request = @"https://bamboo.bistudio.com/chain/admin/deleteStage.action";
            string postParams = string.Format("buildKey={0}-{1}&stageId={2}&save=Confirm&atl_token=" + atl_token, projKey, buildKey, stageId);

            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void ShareAllArtifactsToAnotherJob(string projKey, string buildKey, string jobKey, string anotherJobKey)
        {
            string requestR = String.Format("https://bamboo.bistudio.com/build/admin/edit/defaultBuildArtifact.action?buildKey={0}-{1}-{2}", projKey, buildKey, jobKey);
            string artifactWebPage = simpleHttp.ExecuteGetRequest(requestR).GetAwaiter().GetResult();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(artifactWebPage);

            var elementName = "artifactDefinition-";

            var artifactIds = htmlDoc.DocumentNode.Descendants()
                                            .Select(element => element.Id)
                                            .Where(elementId => elementId.Contains(elementName))
                                            .Select(parsedId => { return parsedId.Trim(elementName.ToCharArray()); })
                                            .ToList();


            var artifactLocations = htmlDoc.DocumentNode.Descendants()
                                            .Where(element => element.Id.Contains(elementName))
                                            .Select(element => element.Descendants("td").Skip(1).First().InnerText)
                                            .ToList();

            for (int i = 0; i < artifactIds.Count(); i++)
            {
                Console.WriteLine("- {0} {1}", artifactIds[i], artifactLocations[i]);

                var request = @"https://bamboo.bistudio.com/ajax/toggleArtifactDefinitionSharing.action";
                var postParams = string.Format("artifactId={0}&buildKey={1}-{2}&save=Confirm&atl_token=" + atl_token, artifactIds[i], projKey, buildKey);
                simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();

                request = @"https://bamboo.bistudio.com/ajax/createArtifactSubscription.action";
                postParams = string.Format("artifactDefinitionId={0}&selectFields=artifactDefinitionId&destination=" + artifactLocations[i] + "&planKey={1}-{2}-{3}&returnUrl=%2Fbuild%2Fadmin%2Fedit%2FdefaultBuildArtifact.action%3FbuildKey%{1}-{2}-{3}&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true", artifactIds[i], projKey, buildKey, anotherJobKey);
                simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
            }
        }

        public void AddA3SVNLinkedRepository(string projKey, string buildKey)
        {
            string request = @"https://bamboo.bistudio.com/chain/admin/config/createRepository.action";
            string postParams = string.Format("planKey={0}-{1}&repositoryId=0&selectedRepository=62619675&selectFields=62619675&decorator=nothing&confirm=true&atl_token=" + atl_token, projKey, buildKey);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void AddPlanVariable(string projKey, string buildKey, string variableName)
        {
            string request = String.Format("https://bamboo.bistudio.com/build/admin/ajax/createPlanVariable.action?planKey={0}-{1}", projKey, buildKey);
            string postParams = string.Format("variableKey={0}&variableValue=&variableValue_password=&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true", variableName);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void AddPlanVariable(string projKey, string buildKey, string variableName, string variableValue)
        {
            string request = String.Format("https://bamboo.bistudio.com/build/admin/ajax/createPlanVariable.action?planKey={0}-{1}", projKey, buildKey);
            string postParams = string.Format("variableKey={0}&variableValue={1}&variableValue_password=&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true", variableName, variableValue);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

     
        public void JobCleanWorkingDirectory(string projKey, string buildKey, string jobKey)
        {
            string request = @"https://bamboo.bistudio.com/build/admin/edit/updateMiscellaneous.action";
            string postParams = string.Format("buildKey={0}-{1}-{2}&cleanWorkingDirectory=true&checkBoxFields=cleanWorkingDirectory&checkBoxFields=custom.buildHangingConfig.enabled&custom.buildHangingConfig.multiplier=&custom.buildHangingConfig.minutesBetweenLogs=&custom.buildHangingConfig.minutesQueueTimeout=&checkBoxFields=custom.ncover.exists&custom.ncover.path=&checkBoxFields=custom.clover.exists&checkBoxFields=custom.clover.historical&checkBoxFields=custom.clover.json&custom.clover.useLocalLicenseKey=true&custom.clover.license=&custom.clover.path=&custom.auto.regex=&custom.auto.label=&save=Save&atl_token=" + atl_token, projKey, buildKey, jobKey);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }
    }
}
