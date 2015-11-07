using System.Collections.Generic;
using HometasksMonitoringPanel.Providers;

namespace HometasksMonitoringPanel.ViewModels
{
    public class ReposViewModel
    {
        public GitHubRepository Repo { get; set; }

        public IEnumerable<Issue> Issues { get; set; }
    }
}