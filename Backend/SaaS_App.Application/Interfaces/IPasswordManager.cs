namespace SaaS_App.Application.Interfaces
{
    public interface IPasswordManager
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
