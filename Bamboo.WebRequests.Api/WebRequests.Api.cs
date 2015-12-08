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
        private string atl_token = "b5386030554fbc452f12f3fe79eb229b93c83410";//  0e5679088f2b8975a5d619f4dba4d2b532168eb1
        private string password;
        private string login;
        private SimpleHttpHelper simpleHttp;

        public SimpleHttpHelper SimpleHttp { get { return simpleHttp; } set { simpleHttp = value; } }
        public string Atl_token { get { return atl_token; } set { atl_token = value; } }

        public WebApi(string login, string password)
        {
            this.simpleHttp = new SimpleHttpHelper(login, password, atl_token);
        }

        public void ClonePlan(string projKey, string buildKey, string clonedPlanKey, string clonedPlanName, string clonedProjKey)
        {
            string request = @"https://bamboo.bistudio.com/build/admin/create/performClonePlan.action";
            string postParams = string.Format("planKeyToClone={0}-{1}&selectFields=planKeyToClone&existingProjectKey={4}&selectFields=existingProjectKey&projectName=&projectKey=&chainName={2}&chainKey={3}&chainDescription=&clonePlan=true&tmp.createAsEnabled=true&checkBoxFields=tmp.createAsEnabled&save=Create&atl_token=" + atl_token, projKey, buildKey, clonedPlanName, clonedPlanKey, clonedProjKey);
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

        public void DeleteTask(string projKey, string buildKey, string TaskName)
        {
            string requestR = String.Format("https://bamboo.bistudio.com/build/admin/edit/editBuildTasks.action?buildKey={0}-{1}", projKey, buildKey);
            var tasksWebPage = simpleHttp.ExecuteGetRequest(requestR).GetAwaiter().GetResult();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(tasksWebPage);

            var elementName = "stage-";
            var selectedStage = htmlDoc.DocumentNode.Descendants()
                                            .SingleOrDefault(element => element.Attributes.Contains("href") &&
                                                             element.Descendants().Any(childElement => childElement.InnerText == TaskName)
                                                            ).Id;

            int stageId = Int32.Parse(selectedStage.Trim(elementName.ToCharArray()));

            //string request = @"https://bamboo.bistudio.com/chain/admin/deleteStage.action";
            //string postParams = string.Format("buildKey={0}-{1}&stageId={2}&save=Confirm&atl_token=" + atl_token, projKey, buildKey, stageId);

            //simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void SetPlanVariable(string projectKey, string buildKey, string variableName, string variableValue)
        {
            string requestR = String.Format("https://bamboo.bistudio.com/chain/admin/config/configureChainVariables.action?buildKey={0}-{1}", projectKey, buildKey);
            var stagesWebPage = simpleHttp.ExecuteGetRequest(requestR).GetAwaiter().GetResult();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(stagesWebPage);

            var elementName = "key_";
            try
            {
                var selectedVariable = htmlDoc.DocumentNode.Descendants("span")
                                                .SingleOrDefault(element => element.Id.Contains(elementName) &&
                                                                 element.InnerText == variableName
                                                                ).Id.Trim(elementName.ToCharArray());
        

            int variableId = Int32.Parse(selectedVariable);
            string request = String.Format("https://bamboo.bistudio.com/build/admin/ajax/updatePlanVariable.action?planKey={0}-{1}", projectKey, buildKey);
            string postParams = string.Format("variableId={0}&variableKey={1}&variableValue={2}&bamboo.successReturnMode=json&decorator=nothing&confirm=true&atl_token=" + atl_token, variableId, variableName, variableValue);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();

            }

            catch (Exception ex)
            {
                //FIX  - SWALLOW FOR NOW - caller should check if variable exists via bamboo sharp, when mass-change, exception would halt all batch
            }
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

                //request = @"https://bamboo.bistudio.com/ajax/createArtifactSubscription.action";
                //postParams = string.Format("artifactDefinitionId={0}&selectFields=artifactDefinitionId&destination=" + artifactLocations[i] + "&planKey={1}-{2}-{3}&returnUrl=%2Fbuild%2Fadmin%2Fedit%2FdefaultBuildArtifact.action%3FbuildKey%{1}-{2}-{3}&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true", artifactIds[i], projKey, buildKey, anotherJobKey);
                //simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
            }
        }

        public void DeleteAllArtifacts(string projKey, string buildKey, string jobKey, string anotherJobKey)
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

                //var request = @"https://bamboo.bistudio.com/ajax/toggleArtifactDefinitionSharing.action";
                //var postParams = string.Format("artifactId={0}&buildKey={1}-{2}&save=Confirm&atl_token=" + atl_token, artifactIds[i], projKey, buildKey);
                //simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
                
               var request = @"https://bamboo.bistudio.com/ajax/deleteArtifactDefinition.action";

                //artifactDefinitionId={0}&selectFields=artifactDefinitionId&destination=" + artifactLocations[i] + "&planKey={1}-{2}-{3}&returnUrl=%2Fbuild%2Fadmin%2Fedit%2FdefaultBuildArtifact.action%3FbuildKey%    atl_token,
                var postParams = string.Format("planKey={1}-{2}-{3}&artifactId={0}&projKey&buildKey&anotherJobKey&atl_token=" + atl_token, artifactIds[i], projKey, buildKey, anotherJobKey);
                simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
            }
        }

        public void UpdateVariable(string projKey, string buildKey, int orderNumber, string variableName)
        {
            string requestR = String.Format("https://bamboo.bistudio.com/branch/admin/config/editChainBranchVariables.action?buildKey={0}-{1}{2}", projKey, buildKey, orderNumber);
            string artifactWebPage = simpleHttp.ExecuteGetRequest(requestR).GetAwaiter().GetResult();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(artifactWebPage);

            var elementName = "key_";
            var selectedVariable = htmlDoc.DocumentNode.Descendants("span");
                                              //.SingleOrDefault(element => element.Id.Contains(elementName) &&
                                              //                 element.InnerText == variableName
                                              //                ).Id.Trim(elementName.ToCharArray());

            //int variableId = Int32.Parse(selectedVariable);



            //for (int i = 0; i < artifactIds.Count(); i++)
            //{
            //    Console.WriteLine("- {0} {1}", artifactIds[i], artifactLocations[i]);

            //    var request = @"https://bamboo.bistudio.com/ajax/toggleArtifactDefinitionSharing.action";
            //    var postParams = string.Format("artifactId={0}&buildKey={1}-{2}&save=Confirm&atl_token=" + atl_token, artifactIds[i], projKey, buildKey);
            //    simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();

            //    //request = @"https://bamboo.bistudio.com/ajax/createArtifactSubscription.action";
            //    //postParams = string.Format("artifactDefinitionId={0}&selectFields=artifactDefinitionId&destination=" + artifactLocations[i] + "&planKey={1}-{2}-{3}&returnUrl=%2Fbuild%2Fadmin%2Fedit%2FdefaultBuildArtifact.action%3FbuildKey%{1}-{2}-{3}&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true", artifactIds[i], projKey, buildKey, anotherJobKey);
            //    //simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
            //}
        }

        public void AddA3SVNLinkedRepository(string projKey, string buildKey)
        {
            string request = @"https://bamboo.bistudio.com/chain/admin/config/createRepository.action";
            string postParams = string.Format("planKey={0}-{1}&repositoryId=0&selectedRepository=62619675&selectFields=62619675&decorator=nothing&confirm=true&atl_token=" + atl_token, projKey, buildKey);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void AddPlanVariable(string projKey, string buildKey, string variableName)
        {
            string request = String.Format("https://bamboo.bistudio.com/build/admin/ajax/createPlanVariable.action?planKey=", projKey, buildKey);
            string postParams = string.Format("variableKey={0}&variableValue=&variableValue_password=&atl_token=" + atl_token + "&bamboo.successReturnMode=json&decorator=nothing&confirm=true", variableName);
            simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
        }

        public void AddPlanVariable(string projKey, string buildKey, string variableName, string variableValue, int num)
        {
            string request = String.Format("https://bamboo.bistudio.com/build/admin/ajax/createPlanVariable.action?planKey={0}-{1}{2}", projKey, buildKey, num);
            string postParams = string.Format("variableKey={0}&selectField=variableKey&variableValue={1}&variableValue_password=bi4T%24gr&atl_token="+atl_token+"&bamboo.successReturnModejson&decorator=nothing&confirm=true", variableName, variableValue);
            
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

        public void TestExportPlugin()
        {
            string request = @"https://bamboo.bistudio.com/rest/trade-depot/1.0/project/JHT";
            //string postParams = string.Format("buildKey={0}-{1}-{2}&cleanWorkingDirectory=true&checkBoxFields=cleanWorkingDirectory&checkBoxFields=custom.buildHangingConfig.enabled&custom.buildHangingConfig.multiplier=&custom.buildHangingConfig.minutesBetweenLogs=&custom.buildHangingConfig.minutesQueueTimeout=&checkBoxFields=custom.ncover.exists&custom.ncover.path=&checkBoxFields=custom.clover.exists&checkBoxFields=custom.clover.historical&checkBoxFields=custom.clover.json&custom.clover.useLocalLicenseKey=true&custom.clover.license=&custom.clover.path=&custom.auto.regex=&custom.auto.label=&save=Save&atl_token=" + atl_token, projKey, buildKey, jobKey);
            //simpleHttp.ExecutePostRequest(request, postParams).GetAwaiter().GetResult();
            var result = simpleHttp.ExecuteGetRequest(request).GetAwaiter().GetResult();
        }

        public void OverrideNthBranchRepository(string projKey, string buildKey, int repositoryNumber)
        {
            string request = String.Format("https://bamboo.bistudio.com/branch/admin/config/saveChainBranchRepository.action?buildKey={0}-{1}{2}", projKey, buildKey, repositoryNumber);
            string param = @"overrideRepository=true&checkBoxFields=overrideRepository&repository.svn.repositoryUrl=https://a3.bistudio.com/svn/A3-branches/A3-VisualUpdate/${bamboo.svnSubdirectory}&repository.svn.username=bamboo_mnisek&repository.svn.authType=password&selectFields=repository.svn.authType&checkBoxFields=temporary.svn.passwordChange&temporary.svn.password=Hjd54EndO64RnjFlRmfgLOK&repository.svn.keyFile=&temporary.svn.passphraseChange=true&temporary.svn.passphrase=&repository.svn.sslKeyFile=&temporary.svn.sslPassphraseChange=true&temporary.svn.sslPassphrase=&checkBoxFields=repository.stash.useShallowClones&checkBoxFields=repository.stash.useRemoteAgentCache&checkBoxFields=repository.stash.useSubmodules&repository.stash.commandTimeout=&checkBoxFields=repository.stash.verbose.logs&checkBoxFields=repository.stash.fetch.whole.repository&checkBoxFields=repository.bitbucket.git.useSubmodules&repository.bitbucket.commandTimeout=&checkBoxFields=repository.bitbucket.verbose.logs&checkBoxFields=repository.bitbucket.fetch.whole.repository&checkBoxFields=repository.svn.useExternals&checkBoxFields=repository.svn.useExport&checkBoxFields=commit.isolation.option&repository.svn.branch.autodetectRootUrl=true&checkBoxFields=repository.svn.branch.autodetectRootUrl&repository.svn.branch.manualRootUrl=&repository.svn.tag.autodetectRootUrl=true&checkBoxFields=repository.svn.tag.autodetectRootUrl&repository.svn.tag.manualRootUrl=&checkBoxFields=repository.git.useShallowClones&checkBoxFields=repository.git.useRemoteAgentCache&checkBoxFields=repository.git.useSubmodules&repository.git.commandTimeout=&checkBoxFields=repository.git.verbose.logs&checkBoxFields=repository.git.fetch.whole.repository&checkBoxFields=repository.github.useSubmodules&repository.github.commandTimeout=&checkBoxFields=repository.github.verbose.logs&checkBoxFields=repository.github.fetch.whole.repository&checkBoxFields=commit.isolation.option&repository.hg.commandTimeout=&checkBoxFields=repository.hg.verbose.logs&checkBoxFields=repository.hg.noRepositoryCache&checkBoxFields=repository.common.quietPeriod.enabled&repository.common.quietPeriod.period=10&repository.common.quietPeriod.maxRetries=5&filter.pattern.option=none&selectFields=filter.pattern.option&filter.pattern.regex=&changeset.filter.pattern.regex=&selectedWebRepositoryViewer=bamboo.webrepositoryviewer.provided:noRepositoryViewer&selectFields=selectedWebRepositoryViewer&webRepository.stash.url=&webRepository.stash.project=&webRepository.stash.repositoryName=&webRepository.fisheyeRepositoryViewer.webRepositoryUrl=&webRepository.fisheyeRepositoryViewer.webRepositoryRepoName=&webRepository.fisheyeRepositoryViewer.webRepositoryPath=&webRepository.genericRepositoryViewer.webRepositoryUrl=&webRepository.genericRepositoryViewer.webRepositoryUrlRepoName=&webRepository.hg.scheme=bitbucket&selectFields=webRepository.hg.scheme&planKey=JHT-ARMORFF0&repositoryId=-1&repositoryName=A3 SVN&selectedRepository=com.atlassian.bamboo.plugin.system.repository:svn&save=Save repository&atl_token="+ atl_token;

            simpleHttp.ExecutePostRequest(request, param).GetAwaiter().GetResult();
        }


        public void DeleteBranch(string projKey, string buildKey, int number)
        {

            

            //string request = String.Format("https://bamboo.bistudio.com/chain/admin/deleteChain.action?returnUrl=/browse/{0}-{1}/editConfig&buildKey={0}-{1}{2}", projKey, buildKey, number);
            //string param = String.Format("returnUrl=/browse/{0}-{1}/editConfig&buildKey={0}-{1}{2}", projKey, buildKey, number);

               string request = String.Format("https://bamboo.bistudio.com/chain/admin/deleteChain!doDelete.action", projKey, buildKey, number);
            string param = String.Format("buildKey={0}-{1}{2}&returnUrl=/browse/{0}-{1}/editConfi&save=Confirm&atl_token="+atl_token, projKey, buildKey, number);

            simpleHttp.ExecutePostRequest(request, param).GetAwaiter().GetResult();
        }


    }
}
