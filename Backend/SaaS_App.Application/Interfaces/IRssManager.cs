namespace SaaS_App.Application.Interfaces
{
    public interface IRssManager
    {
        Task<List<string>> ReadUrlsAsync(List<string> urls);
        string GetFeedUrl(string url);
    }
}
