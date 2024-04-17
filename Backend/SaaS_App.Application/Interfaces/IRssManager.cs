namespace SaaS_App.Application.Interfaces
{
    public interface IRssManager
    {
        Task<string> ReadNewFeedAsync(string url);
    }
}
