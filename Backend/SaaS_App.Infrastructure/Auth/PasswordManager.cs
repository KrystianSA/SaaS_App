using Microsoft.AspNetCore.Identity;
using SaaS_App.Application.Interfaces;

namespace SaaS_App.Infrastructure.Auth
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IPasswordHasher<DummyUser> _passwordHasher;

        public PasswordManager(IPasswordHasher<DummyUser> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(new DummyUser(), password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(new DummyUser(), hashedPassword, providedPassword);

            if(verificationResult == PasswordVerificationResult.Success ||
                verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                return true;
            }
            return false;
        }

        public class DummyUser
        {
        }
    }
}
