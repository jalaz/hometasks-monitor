using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HometasksMonitoringPanel.Helpers;
using HometasksMonitoringPanel.Providers;
using HometasksMonitoringPanel.ViewModels;

namespace HometasksMonitoringPanel.Controllers
{
    [Route("{action=Index}")]
    public class HomeController : Controller
    {
        private readonly IRepositoryProvider _repositories;
        private readonly IIssuesProvider _issues;
        private readonly IHometasksProvider _hometasks;
        private readonly ICouchProvider _couchProvider;

        public HomeController(IRepositoryProvider repositories, IIssuesProvider issues, IHometasksProvider hometasks, ICouchProvider couchProvider)
        {
            _repositories = repositories;
            _issues = issues;
            _hometasks = hometasks;
            _couchProvider = couchProvider;
        }

        public async Task<ActionResult> Index()
        {
            var repos = _repositories.GetAll();
            var tasks = _hometasks.GetHometaskTitles();

            var reposInfo = await Task.WhenAll(repos.AsParallel()                                                
                                                    .Select(async r => new ReposViewModel()
                                                    {
                                                        Repo = r,
                                                        Issues = await GetIssues(r.RelativeUrl, tasks)
                                                    }));

            var statuses = reposInfo.SelectMany(repo => repo.Issues.Select(i => new { Key = Tuple.Create(repo.Repo.Name, i.Title), Value = i }))
                                    .DistinctBy(n => n.Key)
                                    .ToDictionary(k => k.Key, v => v.Value);

            return View(new HometasksStatusViewModel { Repos = repos, Tasks = tasks, Issues = statuses });
        }

        private async Task<IEnumerable<Issue>> GetIssues(string repository, string[] tasks)
        {
            var issues = await _issues.GetIssuesAsync(repository);

            return await Task.WhenAll(issues.Where(i => tasks.Contains(i.Title))
                             .Select(async issue => new Issue
                             {
                                 Title = issue.Title,
                                 State = issue.State,
                                 Number = issue.Number,
                                 ClosedAt = issue.ClosedAt,
                                 CreatedAt = issue.CreatedAt,
                                 CommentsCount = issue.CommentsCount,
                                 UpdatedAt = issue.UpdatedAt,
                                 Creator = issue.CreatedAt,
                                 Url = issue.Url,
                                 Status = await GetStatus(repository, issue)
                             }));
        }

        private async Task<string> GetStatus(string repository, Issue issue)
        {
            if (issue.CommentsCount == 0)
            {
                return issue.State;
            }
            var couches = _couchProvider.GetAll();
            var comments = await _issues.GetCommentsAsync(repository, issue.Number);
            return CouchMentioned(comments, couches, new [] { "Проверено" }) ? "verified" : 
                   CouchCommentedLast(couches, comments) ? "reviewed" : issue.State;
        }

        private static bool CouchCommentedLast(string[] couches, IssueComment[] comments)
        {
            return couches.Contains(comments.Select(c => c.User.Login).Last());
        }

        private static bool CouchMentioned(IssueComment[] comments, IEnumerable<string> couches, IEnumerable<string> keyPhrase)
        {
            return comments.Any(c => couches.Contains(c.User.Login) && keyPhrase.Any(phrase => c.Body.Contains(phrase)));
        }
    }
}