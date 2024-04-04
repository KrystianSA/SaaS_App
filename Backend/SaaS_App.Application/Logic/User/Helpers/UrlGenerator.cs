namespace SaaS_App.Application.Logic.User.Helpers
{
    public static class UrlGenerator
    {
        public static string GenerateUrl(string domainName, string site, Dictionary<string, string> parameters)
        {
            return domainName + "/" + site + "?" + string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
        }
    }
}
