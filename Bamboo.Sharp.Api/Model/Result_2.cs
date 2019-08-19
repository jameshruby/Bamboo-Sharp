using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bamboo.Sharp.Api.Model
{
    public enum BuildState
    {
        Successful,
        Failed,
        Unknown,
    }

    public class Progress
    {
        public bool IsValid { get; set; }
        public bool IsUnderAverageTime { get; set; }
        public float PercentageCompleted { get; set; }
        public string percentageCompletedPretty { get; set; }
        public string prettyTimeRemaining { get; set; }
        public string prettyTimeRemainingLong { get; set; }
        public int averageBuildDuration { get; set; }
        public string prettyAverageBuildDuration { get; set; }
        public int buildTime { get; set; }
        public string prettyBuildTime { get; set; }
    }

    public class Result
    {
        public Plan Plan { get; set; }
        public string PlanName { get; set; }

        public string LifeCycleState { get; set; }
        public string Id { get; set; }

        public DateTime BuildStartedTime { get; set; }
        public string PrettyBuildStartedTime { get; set; }
        public DateTime BuildCompletedTime { get; set; }
        public DateTime BuildCompletedDate { get; set; }
        public string PrettyBuildCompletedTime { get; set; }

        public int BuildDurationInSeconds { get; set; }
        public int BuildDuration { get; set; }
        public string BuildDurationDescription
        {
            get; set;
        }
        public string BuildRelativeTime { get; set; }

        public int VcsRevisionKey { get; set; }
        //"vcsRe visions"
        public string BuildTestSummary { get; set; }
        public int SuccessfulTestCount { get; set; }
        public int FailedTestCount { get; set; }
        public int QuarantinedTestCount { get; set; }
        public int SkippedTestCount { get; set; }


        public string Number { get; set; }

        public bool Continuable { get; set; }
        public bool OnceOff { get; set; }
        public bool Restartable { get; set; }
        public bool Finished { get; set; }
        public bool Successful { get; set; }


        public string ReasonSummary { get; set; }

        public string ProjectName { get; set; }

        public Artifacts Artifacts { get; set; }
        public Comments Comments { get; set; }
        public Labels Labels { get; set; }
        public JiraIssues JiraIssues { get; set; }
        public Stages Stages { get; set; }

        public string Key { get; set; }
        public string BuildResultKey { get; set; }
        public string BuildState { get; set; }
        public BuildState State { get; set; }

        public Progress Progress { get; set; }


        public string BuildUser { get; set; }

        private string buildReason;
        public string BuildReason
        {
            get
            {
                return buildReason;
            }

            set
            {
                buildReason = value;
                //temp node created to for multiple elements appearing in the string
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                var temp = doc.CreateElement("temp");
                temp.InnerHtml = buildReason;
                BuildUser = temp.ChildNodes.LastOrDefault().InnerText ?? "Unknown";
            }
        }
    }
}
