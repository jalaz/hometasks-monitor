namespace HometasksMonitoringPanel.Configs
{
    public class GithubConfig
    {
        public string BaseApiUrl { get; }
        public string Token { get; }
        public string BaseSiteUrl { get; }

        public GithubConfig(string apiUrl, string siteUrl, string token)
        {
            BaseApiUrl = apiUrl;
            BaseSiteUrl = siteUrl;
            Token = token;
        }
    }
}