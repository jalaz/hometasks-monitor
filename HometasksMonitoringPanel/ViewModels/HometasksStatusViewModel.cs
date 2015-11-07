using System;
using System.Collections.Generic;
using HometasksMonitoringPanel.Providers;

namespace HometasksMonitoringPanel.ViewModels
{
    public class HometasksStatusViewModel
    {
        public IEnumerable<string> Tasks { get; set; }
        public GitHubRepository[] Repos { get; set; }
        public Dictionary<Tuple<string, string>, Issue> Issues { get; set; }

        public Issue GetIssue(string repo, string task)
        {
            Issue issue = null;
            Issues.TryGetValue(Tuple.Create(repo, task), out issue);
            return issue;
        }

        public string GetStatus(string repo, string task)
        {
            var issue = GetIssue(repo, task);
            return issue == null ? "not found" : issue.Status;
        }

        public string GetUrl(string repo, string task)
        {
            var issue = GetIssue(repo, task);
            return issue == null ? "#" : issue.Url;
        }
    }
}