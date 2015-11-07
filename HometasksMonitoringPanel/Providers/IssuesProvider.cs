using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HometasksMonitoringPanel.Configs;
using Newtonsoft.Json;

namespace HometasksMonitoringPanel.Providers
{
    public class GithubIssuesProvider : IIssuesProvider, IDisposable
    {
        private readonly GithubConfig _config;
        private readonly HttpClient _client;

        public GithubIssuesProvider(GithubConfig config)
        {
            _config = config;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_config.BaseApiUrl)
            };
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Fiddler");
        }

        public async Task<Issue[]> GetIssuesAsync(string repository)
        {
            var response = await _client.GetAsync($"/repos/{repository}/issues?state=all&access_token={_config.Token}");

            return await CreateResponse<Issue[]>(response);
        }

        public async Task<IssueComment[]> GetCommentsAsync(string repository, string issueNumber)
        {
            var response = await _client.GetAsync($"/repos/{repository}/issues/{issueNumber}/comments?access_token={_config.Token}");

            return await CreateResponse<IssueComment[]>(response);
        }

        private async Task<T> CreateResponse<T>(HttpResponseMessage response)
        {
            var responseMsg = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ResponseException($"Response from github was not successful with the following message: {responseMsg}");
            }

            return JsonConvert.DeserializeObject<T>(responseMsg);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }

    public class ResponseException : Exception
    {
        public ResponseException(string msg) : base(msg)
        {
        }
    }

    public class ServiceResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public string StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }
    }

    public interface IIssuesProvider
    {
        Task<Issue[]> GetIssuesAsync(string repository);
        Task<IssueComment[]> GetCommentsAsync(string repository, string issueNumber);
    }

    public class IssueComment
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("user")]
        public GitUser User { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

    public class GitUser
    {
        [JsonProperty("login")]
        public string Login { get; set; }
    }

    public class Issue
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("comments")]
        public int CommentsCount { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("closed_at")]
        public string ClosedAt { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }

        public string Status { get; set; }
    }
}