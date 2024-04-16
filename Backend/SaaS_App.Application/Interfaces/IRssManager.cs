namespace SaaS_App.Application.Interfaces
{
    public interface IRssManager
    {
        Task<string> ReadAsync(string url);
    }
}
